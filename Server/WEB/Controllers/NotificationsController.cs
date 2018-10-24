using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Twilio.Jwt.AccessToken;
using WEB.Infrastructure.Hubs;
using WEB.Models.Hubs;

namespace WEB.Controllers
{
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly IHubContext<ApplicationHub> _hubContext;

        public NotificationsController(IHubContext<ApplicationHub> hubContext)
        {
            this._hubContext = hubContext;
        }

        [HttpPost]
        [Route("/api/[controller]/message")]
        public async Task Post(NotificationHubModel model)
        {
            await _hubContext.Clients.All.SendAsync("notificationReceived", "rest", model.Message);
        }

        [HttpGet]
        [Route("/api/[controller]/token")]
        public IActionResult Get([FromQuery]string identity)
        {
            // These values are necessary for any access token
            const string twilioAccountSid = "ACaac624ef42e8299f0e3bb25497dc839d";
            const string twilioApiKey = "SK7ba2eaaf7a8ce751e4471ec7c290be69";
            const string twilioApiSecret = "iVL4kfIKnWcX2Q3ivhjSfoUbXDf4S3rO";

//            // These are specific to Chat
//            const string serviceSid = "ISXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
//            const string identity = "user@example.com";

            // Create an Chat grant for this token

            var grant = new VideoGrant();
            var grants = new HashSet<IGrant>
            {
                { grant }
            };

            // Create an Access Token generator
            var token = new Token(
                twilioAccountSid,
                twilioApiKey,
                twilioApiSecret,
                identity,
                grants: grants);

            return Ok(new {Token = token.ToJwt()});
        }
    }
}