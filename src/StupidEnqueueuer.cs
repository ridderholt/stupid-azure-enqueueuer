﻿using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;

namespace laget.azure_enqueueuer
{
    public interface IStupidEnqueueuer
    {
        void Enqueue(string queueName, dynamic msg);
        void Enqueue(string queueName, string msg);
    }

    public class StupidEnqueueuer : IStupidEnqueueuer
    {
        readonly CloudQueueClient _client;

        public StupidEnqueueuer(string cloudStorageAccountConnStr)
        {
            var storageAccount = CloudStorageAccount.Parse(cloudStorageAccountConnStr);
            _client = storageAccount.CreateCloudQueueClient();
        }

        public void Enqueue(string queueName, dynamic msg)
        {
            Enqueue(queueName, JsonConvert.SerializeObject(msg));
        }

        public void Enqueue(string queueName, string msg)
        {
            var queue = _client.GetQueueReference(queueName);
            queue.CreateIfNotExists();

            var message = new CloudQueueMessage(msg);
            queue.AddMessage(message);
        }
    }
}
