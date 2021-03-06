using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace asp_back.hubs {

    public class ChatHub : Hub {
        public async Task NewMessage(string username, string message)
        {
            await Clients.All.SendAsync("messageReceived", username, message);
            // ("Reached here");
        }
        public async Task SendMessage (string user, string message) {
            await Clients.All.SendAsync ("ReceiveMessage", user, message);
        }

        public async Task SendScore (string user, number score){
            await Clients.All.SendAsync("sendScore", user, score);
        }

        public Task SendMessageToCaller (string message) {
            return Clients.Caller.SendAsync ("ReceiveMessage", message);
        }

        public Task SendMessageToGroups (string message) {
            List<string> groups = new List<string> () { "SignalR Users" };
            return Clients.Groups (groups).SendAsync ("ReceiveMessage", message);
        }

        public override async Task OnConnectedAsync () {
            await Groups.AddToGroupAsync (Context.ConnectionId, "SignalR Users");
            await base.OnConnectedAsync ();
        }

        public override async Task OnDisconnectedAsync (Exception exception) {
            await Groups.RemoveFromGroupAsync (Context.ConnectionId, "SignalR Users");
            await base.OnDisconnectedAsync (exception);
        }
    }
}