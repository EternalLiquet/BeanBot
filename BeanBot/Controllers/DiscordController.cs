using Discord.WebSocket;
using MicroserviceBotsUtil.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeanBot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscordController : ControllerBase
    {
        private readonly DiscordShardedClient _discordClient;

        public DiscordController(DiscordShardedClient discordClient)
        {
            _discordClient = discordClient;
        }

        // POST: /api/Discord/Reply
        [HttpPost("Reply")]
        public IActionResult Reply([FromBody] DiscordReply messageBody)
        {

            Log.Information($"Reply recieved: {messageBody}");
            return Ok();
        }
    }
}
