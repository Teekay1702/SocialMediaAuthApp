using Microsoft.AspNetCore.SignalR;
using SocialMediaAuthApp.Models;

namespace SocialMediaAuthApp.Hubs
{
    public class TaskHub : Hub
    {
        public async Task SendTaskUpdate(TaskModel task)
        {
            await Clients.All.SendAsync("ReceiveTaskUpdate", task);
        }
    }
}
