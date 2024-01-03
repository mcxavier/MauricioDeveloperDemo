using Azure.Storage.Queues;
using LinxIO.Interfaces;
using Microsoft.Extensions.Configuration;
using System;

namespace LinxIO.Queue.Services
{
    public class QueueService : IQueueService
    {
        private readonly string _connectionString;
        public QueueService(IConfiguration configuration)
        {
            _connectionString = configuration["AzureWebJobsStorage"];
        }
        public void SendMessage(string queueName, string message)
        {
            var queueClient = new QueueClient(_connectionString, queueName, new QueueClientOptions { MessageEncoding = QueueMessageEncoding.Base64 });
            queueClient.CreateIfNotExists();
            if (queueClient.Exists())
            {
                queueClient.SendMessage(message);
            }
        }
    }
}
