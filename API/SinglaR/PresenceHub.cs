using System;
using System.Threading.Tasks;
using API.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.SinglaR
{
	[Authorize]
	public class PresenceHub : Hub
	{
		private readonly PresenceTracker _presenceTracker;

		public PresenceHub(PresenceTracker presenceTracker)
		{
			_presenceTracker = presenceTracker;
		}

		public override async Task OnConnectedAsync()
		{
			var isOnline = await _presenceTracker.UserConected(Context.User.GetUserName(), Context.ConnectionId);
			if (isOnline)
			{
				await Clients.Others.SendAsync("UserIsOnline", Context.User.GetUserName());
			}

			var currentUsers = await _presenceTracker.GetOnlineUsers();
			await Clients.Caller.SendAsync("GetOnlineUsers", currentUsers);
		}

		public override async Task OnDisconnectedAsync(Exception exception)
		{
			var isOfline = await _presenceTracker.UserDisconected(Context.User.GetUserName(), Context.ConnectionId);
			if (isOfline)
			{
				await Clients.Others.SendAsync("UserIsOffline", Context.User.GetUserName());
			}

			await base.OnDisconnectedAsync(exception);
		}
	}
}