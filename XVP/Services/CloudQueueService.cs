namespace XVP.Presentation.MVC.Services
{
    using System;
    using System.Configuration;
    using System.Threading.Tasks;

    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Queue;

    using Newtonsoft.Json;

    using XVP.Domain.Commands;
    using XVP.Infrastructure.Shared.Enum;

    public class CloudQueueService : ICloudQueueService
    {
        private CloudQueue requestQueue;

        public CloudQueueService(AzureQueues queue)
        {
            this.InitializeStorage(queue);
        }

        public async Task AddMessageAsync(ICommand command)
        {
            // Allows us to capture ICommand concrete type in $Type json param and then deserialize to the correct concrete type
            var settings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Objects };
            var serialized = JsonConvert.SerializeObject((ICommand)command, Formatting.None, settings);
            var queueMessage = new CloudQueueMessage(serialized);
           
            await this.requestQueue.AddMessageAsync(queueMessage);
        }

        private void InitializeStorage(AzureQueues queue)
        {
            try
            {
                var storageAccount =
                    CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["AzureWebJobsStorage"].ToString());
                var queueClient = storageAccount.CreateCloudQueueClient();
                this.requestQueue = queueClient.GetQueueReference("command-queue");
                this.requestQueue.CreateIfNotExists();
            }
            catch (Exception ex)
            {
               throw new Exception("An error occured in initializing the storage queues with exception message " + ex.Message);
            }
        }

        
    }
}
