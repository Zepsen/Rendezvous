using System.Threading.Tasks;
using BLL.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WEB.Infrastructure.Hubs;
using WEB.Models.Hubs;

namespace WEB.Controllers
{
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationsController(IHubContext<NotificationHub> hubContext)
        {
            this._hubContext = hubContext;
        }

        [HttpPost]
        [Route("/api/[controller]/message")]
        public async Task Post(NotificationHubModel model)
        {
            await _hubContext.Clients.All.SendAsync("notificationReceived", "rest", model.Message);
        }
    }
}