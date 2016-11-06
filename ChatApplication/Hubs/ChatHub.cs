using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

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
		public static List<KeyValuePair<string, ChatModel>> messanges;

		public object GetHistory(string room)
		{
			return messanges.Where((arg) => arg.Key.Equals(room));
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

        public Task JoinRoom(string roomName)
        {
            return Groups.Add(Context.ConnectionId, roomName);
        }

        public Task LeaveRooom(string roomName)
        {
            return Groups.Remove(Context.ConnectionId, roomName);
        }
    }
}