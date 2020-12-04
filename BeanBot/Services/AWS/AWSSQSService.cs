using Amazon.SQS.Model;
using BeanBot.Helpers;
using BeanBot.Models;
using Discord.Commands;
using Discord.WebSocket;
using MicroserviceBotsUtil.Entities;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeanBot.Services.AWS
{
    public interface IAWSSQSService
    {
        Task<bool> PostMessageAsync(SocketCommandContext message);
        Task<List<AllMessage>> GetAllMessagesAsync();
        Task<bool> DeleteMessageAsync(DeleteMessage deleteMessage);
    }
    public class AWSSQSService : IAWSSQSService
    {
        private readonly IAWSSQSHelper _AWSSQSHelper;
        public AWSSQSService(IAWSSQSHelper AWSSQSHelper)
        {
            this._AWSSQSHelper = AWSSQSHelper;
        }
        public async Task<bool> PostMessageAsync(SocketCommandContext context)
        {
            try
            {
                return await _AWSSQSHelper.SendMessageAsync(new DiscordMessage(context.Message, context.Guild.Id));
            }
            catch (Exception ex)
            {
                Log.Error($"Error posting message to SQS: {ex.Message}");
                return false;
            }
        }
        public async Task<List<AllMessage>> GetAllMessagesAsync()
        {
            List<AllMessage> allMessages = new List<AllMessage>();
            try
            {
                List<Message> messages = await _AWSSQSHelper.ReceiveMessageAsync();
                allMessages = messages.Select(c => new AllMessage { MessageId = c.MessageId, ReceiptHandle = c.ReceiptHandle, UserDetail = JsonConvert.DeserializeObject<UserDetail>(c.Body) }).ToList();
                return allMessages;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteMessageAsync(DeleteMessage deleteMessage)
        {
            try
            {
                return await _AWSSQSHelper.DeleteMessageAsync(deleteMessage.ReceiptHandle);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
