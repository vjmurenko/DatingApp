using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.SinglaR
{
	public class PresenceTracker
	{
		public Dictionary<string, List<string>> OnlineUsers = new Dictionary<string, List<string>>();

		public Task<bool> UserConected(string username, string connectionId)
		{
			bool isOnline = false;

			lock (OnlineUsers)
			{
				if (OnlineUsers.ContainsKey(username))
				{
					OnlineUsers[username].Add(connectionId);
				}
				else
				{
					OnlineUsers.Add(username, new List<string> { connectionId });
					isOnline = true;
				}
			}

			return Task.FromResult(isOnline);
		}

		public Task<bool> UserDisconected(string username, string coneectionId)
		{
			bool isOnline = false;
			lock (OnlineUsers)
			{
				if (!OnlineUsers.ContainsKey(username)) return Task.FromResult(isOnline);

				OnlineUsers[username].Remove(coneectionId);

				if (OnlineUsers[username].Count == 0)
				{
					OnlineUsers.Remove(username);
					isOnline = true;
				}
			}

			return Task.FromResult(isOnline);
		}

		public Task<string[]> GetOnlineUsers()
		{
			string[] onlineUsers;

			lock (OnlineUsers)
			{
				onlineUsers = OnlineUsers.OrderBy(u => u.Key).Select(u => u.Key).ToArray();
			}

			return Task.FromResult(onlineUsers);
		}

		public Task<List<string>> GetConnectionsForUser(string username)
		{
			List<string> connectionIds;
			lock (OnlineUsers)
			{
				connectionIds = OnlineUsers.GetValueOrDefault(username);
			}

			return Task.FromResult(connectionIds);
		}
	}
}