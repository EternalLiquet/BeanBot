using Amazon.SQS;
using Amazon.SQS.Model;
using BeanBot.Models;
using MicroserviceBotsUtil.Entities;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeanBot.Helpers
{
    public interface IAWSSQSHelper
    {
        Task<bool> SendMessageAsync(DiscordMessage message);
        Task<List<Message>> ReceiveMessageAsync();
        Task<bool> DeleteMessageAsync(string messageReceiptHandle);
    }

    public class AWSSQSHelper : IAWSSQSHelper
    {
        private readonly IAmazonSQS _sqs;
        private readonly ServiceConfiguration _settings;
        public AWSSQSHelper(
           IAmazonSQS sqs,
           IOptions<ServiceConfiguration> settings)
        {
            this._sqs = sqs;
            this._settings = settings.Value;
        }
        public async Task<bool> SendMessageAsync(DiscordMessage message)
        {
            try
            {
                string jsonMessage = JsonConvert.SerializeObject(message);
                var sendRequest = new SendMessageRequest(_settings.AWSSQS.QueueUrl, jsonMessage);
                sendRequest.MessageGroupId = Guid.NewGuid().ToString();
                var sendResult = await _sqs.SendMessageAsync(sendRequest);
                return sendResult.HttpStatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                Log.Error($"Error posting message to SQS: {ex.Message}");
                return false;
            }
        }
        public async Task<List<Message>> ReceiveMessageAsync()
        {
            try
            {
                //Create New instance  
                var request = new ReceiveMessageRequest
                {
                    QueueUrl = _settings.AWSSQS.QueueUrl,
                    MaxNumberOfMessages = 10,
                    WaitTimeSeconds = 5
                };
                //CheckIs there any new message available to process  
                var result = await _sqs.ReceiveMessageAsync(request);

                return result.Messages.Any() ? result.Messages : new List<Message>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> DeleteMessageAsync(string messageReceiptHandle)
        {
            try
            {
                //Deletes the specified message from the specified queue  
                var deleteResult = await _sqs.DeleteMessageAsync(_settings.AWSSQS.QueueUrl, messageReceiptHandle);
                return deleteResult.HttpStatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
