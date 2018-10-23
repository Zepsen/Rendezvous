using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using WEB.Models.Hubs;

namespace WEB.Infrastructure.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task Notification(NotificationHubModel model)
        {
            await Clients.All.SendAsync("notificationReceived", "server", model.Message);
        }
    }
}