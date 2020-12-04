using BeanBot.Models;
using BeanBot.Services.AWS;
using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeanBot.Modules
{
    [Name("Test Commands")]
    public class MemeModule : InteractiveBase
    {
        private readonly IAWSSQSService _AWSSQSService;

        public MemeModule(IAWSSQSService AWSSQSService)
        {
            _AWSSQSService = AWSSQSService;
        }

        [Command("8ball")]
        [Summary("Let me predict your future.. for a price")]
        [Alias("fortune")]
        [RequireBotPermission(ChannelPermission.SendMessages)]
        public async Task EightBall([Remainder] string question)
        {
            var result = await _AWSSQSService.PostMessageAsync(Context);
            await ReplyAsync((result) ? "Message successfully posted to AWS SQS Queue" : "Something has gone wrong posting to AWS SQS Queue");
        }
    }
}
