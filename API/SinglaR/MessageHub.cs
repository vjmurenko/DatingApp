using System;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTO;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;

namespace API.SinglaR
{
	public class MessageHub : Hub
	{

		private readonly IMapper _mapper;
	
		private readonly PresenceTracker _presenceTracker;
		private readonly IHubContext<PresenceHub> _presenceHub;
		private readonly IUnityOfWork _unityOfWork;

		public MessageHub(IMapper mapper, PresenceTracker presenceTracker, IHubContext<PresenceHub> presenceHub, IUnityOfWork unityOfWork)
		{
			_mapper = mapper;
			_presenceTracker = presenceTracker;
			_presenceHub = presenceHub;
			_unityOfWork = unityOfWork;
		}

		public override async Task OnConnectedAsync()
		{
			var context = Context.GetHttpContext();
			var otherUser = context.Request.Query["user"].ToString();
			var groupName = GetGroupName(Context.User.GetUserName(), otherUser);
			await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
			var group = await AddToGroup(groupName);
			await Clients.Group(groupName).SendAsync("UpdatedGroup", group);

			var messages = await _unityOfWork.MessageRepository.GetMessageThread(Context.User.GetUserName(), otherUser);
			if (_unityOfWork.HasChanges()) await _unityOfWork.Complete();

			await Clients.Caller.SendAsync("ReceiveMessageThread", messages);
		}

		public override async Task OnDisconnectedAsync(Exception exception)
		{
			var group  = await RemoveFromMessageGroup();
			await Clients.Group(group.Name).SendAsync("UpdatedGroup", group);
			await base.OnDisconnectedAsync(exception);
		}

		public async Task SendMessage(CreateMessageDto createMessageDto)
		{
			var username = Context.User.GetUserName();

			if (username == createMessageDto.RecipientUserName)
			{
				throw new HubException("You can't send message to yourself");
			}

			var sender = await _unityOfWork.UserRepository.GetUserByName(username);
			var recipient = await _unityOfWork.UserRepository.GetUserByName(createMessageDto.RecipientUserName);

			if (recipient == null) throw new HubException("Not found");

			var message = new Message()
			{
				Content = createMessageDto.Content,
				Recipient = recipient,
				Sender = sender,
				RecipientUsername = recipient.UserName,
				SenderUsername = sender.UserName
			};
			var groupName = GetGroupName(sender.UserName, recipient.UserName);
			var group = await _unityOfWork.MessageRepository.GetMessageGroup(groupName);

			if (group.Connections.Any(c => c.UserName == recipient.UserName))
			{
				message.DateRead = DateTime.UtcNow;
			}
			else
			{
				var connections = await _presenceTracker.GetConnectionsForUser(recipient.UserName);
				if (connections != null)
				{
					await _presenceHub.Clients.Clients(connections).SendAsync("NewMessageReceived", new { username = sender.UserName, knownAs = sender.KnownAs });
				}
			}

			_unityOfWork.MessageRepository.AddMessage(message);

			if (await _unityOfWork.Complete())
			{
				await Clients.Group(groupName).SendAsync("NewMessage", _mapper.Map<MessageDto>(message));
			}
		}

		private async Task<Group> AddToGroup(string groupName)
		{
			var group = await _unityOfWork.MessageRepository.GetMessageGroup(groupName);
			var connection = new Connection(Context.ConnectionId, Context.User.GetUserName());
			if (group == null)
			{
				group = new Group(groupName);
				_unityOfWork.MessageRepository.AddGroup(group);
			}

			group.Connections.Add(connection);

			if (await _unityOfWork.Complete()) return group;

			throw new HubException("Failed to join the group");
		}

		private async Task<Group> RemoveFromMessageGroup()
		{
			var group = await _unityOfWork.MessageRepository.GetGroupForConnection(Context.ConnectionId);
			var connection = group.Connections.FirstOrDefault(c => c.ConnectionId == Context.ConnectionId);
			_unityOfWork.MessageRepository.RemoveConnection(connection);

			if (await _unityOfWork.Complete()) return group;

			throw new HubException("Failed to remove from group");
		}

		private string GetGroupName(string caller, string receiver)
		{
			var compareResult = string.CompareOrdinal(caller, receiver) < 0;
			return compareResult ? $"{caller}-{receiver}" : $"{receiver}-{caller}";
		}
	}
}