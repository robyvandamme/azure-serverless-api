using System;
using System.Threading.Tasks;
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
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "messages")] HttpRequest req,
            [Queue("outqueue"),StorageAccount("AzureWebJobsStorage")] ICollector<Message> msg,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request");

            var json = await req.ReadAsStringAsync();
            var newMessage = JsonConvert.DeserializeObject<NewMessage>(json);
            var message = MessageHandler.Handle(newMessage);

            if (message == null)
            {
                return new UnprocessableEntityResult();
            }

            // adds the message to the queue
            msg.Add(message);

            return new CreatedResult(new Uri($"http://localhost:7071/api/messages/{message.Id.ToString()}"), message);
        }
    }
}
