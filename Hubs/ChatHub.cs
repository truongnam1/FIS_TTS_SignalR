using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace SignalR1.Hubs
{
    public class ChatHub : Hub
    {
        private static int connectAmount = 0;
        public async Task SendMessage(string username, string message)
        {
            Console.WriteLine(Context.UserIdentifier);
            await Clients.All.SendAsync("ReceiveMessage", username, message);
        }

        public async Task SendMessageOther(string username, string message)
        {
            await Clients.Others.SendAsync("ReceiveMessage", username, message);
        }

        public async Task SendMessageCaller(string username, string message)
        {
            await Clients.Caller.SendAsync("ReceiveMessage", username, message);
            
        }

        public override async Task OnConnectedAsync()
        {
            connectAmount++;
            await SendUserOnlineAmount();
            await Clients.All.SendAsync("connected", $"id connect: {Context.ConnectionId}");
        }

        //
        // Summary:
        //     Called when a connection with the hub is terminated.
        //
        // Returns:
        //     A System.Threading.Tasks.Task that represents the asynchronous disconnect.
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            connectAmount--;
            await SendUserOnlineAmount();
            await Clients.All.SendAsync("disconnected", $"id disconnect: {Context.ConnectionId}");

        }

        private async Task SendUserOnlineAmount()
        {
            await Clients.All.SendAsync("userOnline", connectAmount);
        }

        public void Abort()
        {
            Context.Abort();
            Context.Items.Add("key", "value");
        }

    }
}
