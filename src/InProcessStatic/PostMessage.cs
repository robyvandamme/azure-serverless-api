using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using InProcessStatic.Handlers;
using InProcessStatic.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace InProcessStatic
{
    public static class PostMessage
    {
        [FunctionName("PostMessage")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "messages")] HttpRequest request,
            [Queue("outqueue"),StorageAccount("AzureWebJobsStorage")] ICollector<Message> messageQueue,
            [Blob("messages", FileAccess.Write, Connection = "AzureWebJobsStorage")] BlobContainerClient containerClient,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request");

            var json = await request.ReadAsStringAsync();
            var newMessage = JsonConvert.DeserializeObject<NewMessage>(json);
            var message = MessageHandler.Handle(newMessage);

            if (message == null)
            {
                return new UnprocessableEntityResult();
            }

            messageQueue.Add(message);

            SaveMessageToBlobStorage(containerClient, message);

            return new CreatedResult(new Uri($"http://localhost:7071/api/messages/{message.Id.ToString()}"), message);
        }

        private static void SaveMessageToBlobStorage(BlobContainerClient containerClient, Message message)
        {
            containerClient.CreateIfNotExists();

            var messageJson = JsonConvert.SerializeObject(message);
            var blobClient = containerClient.GetBlobClient(message.Id.ToString());
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(messageJson));
            blobClient.Upload(stream);
        }
    }
}
