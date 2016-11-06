using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Linq.Expressions;

namespace ChatApplication.Hubs
{
	public class ChatModel
	{
		string personName;

		public string PersonName
		{
			get
			{
				return personName;
			}

			set
			{
				personName = value;
			}
		}

		public string TextBody { get; set; }


		public bool IsMine
		{
			get
			{
				return false;
			}
		}

		public string ShortDate
		{
			get
			{
				return FullDate.ToString("t");
			}
		}

		public DateTime FullDate { get; set; }
	}

    public class ChatHub : Hub
    {
		public static List<KeyValuePair<string, ChatModel>> messanges = new List<KeyValuePair<string, ChatModel>>();
		public static List<string> users = new List<string>();
		public static List<string> registredUsers = new List<string>();


		public IEnumerable<KeyValuePair<string, ChatModel>> GetHistory(string room)
		{
			if ((messanges == null) || (messanges.Count == 0))
				return default(IEnumerable<KeyValuePair<string, ChatModel>>);
			return messanges.Where((arg) => arg.Key.Equals(room)).AsEnumerable();
		}

		public IEnumerable<string> GetOnlineUsers()
		{
			if ((users == null) || (users.Count == 0))
				return default(IEnumerable<string>);
			return users.AsEnumerable();
		}


		public IEnumerable<string> GetUsers()
		{
			if ((registredUsers == null) || (registredUsers.Count == 0))
				return default(IEnumerable<string>);
			return registredUsers.AsEnumerable();
		}
		
        public void SendMessage(string name, string message, string fullDate, string roomName)
        {
			messanges.Add(new KeyValuePair<string, ChatModel>(roomName , new ChatModel()
			{
				FullDate = DateTime.Parse(fullDate),
				TextBody = message,
				PersonName = name,
			}));
            Clients.Group(roomName).GetMessage(name, message, fullDate);
        }

        public Task JoinRoom(string roomName, string name)
        {
			if (!registredUsers.Contains(name))
			{
				registredUsers.Add(name);
				registredUsers.Distinct();
			}
			if (!users.Contains(name))
			{
				users.Add(name);
				users.Distinct();
			}
            return Groups.Add(Context.ConnectionId, roomName);
        }

        public Task LeaveRooom(string roomName, string name)
        {
			users.Remove(name);
			users.Distinct();
            return Groups.Remove(Context.ConnectionId, roomName);
        }
    }
}