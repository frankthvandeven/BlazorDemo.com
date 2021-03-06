using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace BlazorDemo.Server.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, "Server added this text. "+ message);
        }
    }
}
