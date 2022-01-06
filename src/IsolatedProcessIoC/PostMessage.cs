using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using IsolatedProcessIoC.Handlers;
using IsolatedProcessIoC.Models;
using IsolatedProcessIoC.Responses;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace IsolatedProcessIoC
{
    public class PostMessage
    {
        private readonly IMessageHandler _messageHandler;
        private readonly ILogger _logger;

        public PostMessage(ILoggerFactory loggerFactory, IMessageHandler messageHandler)
        {
            _messageHandler = messageHandler;
            _logger = loggerFactory.CreateLogger<PostMessage>();
        }

        [OpenApiOperation(operationId: "PostMessage", tags: new[] { "Messages" }, Summary = "Post a message", Description = "Saves a message to storage", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created, contentType: "application/json", bodyType: typeof(Message), Summary = "The response", Description = "The created message with its ID")]
        [OpenApiResponseWithoutBody(HttpStatusCode.UnprocessableEntity, Summary = "Invalid message")]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(NewMessage))]
        [Function("PostMessage")]
        public async Task<MultiResponse> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "messages")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request");

            var newMessage = await req.ReadFromJsonAsync<NewMessage>();
            var message = _messageHandler.Handle(newMessage);

            if (message == null)
            {
                var errorResponse = req.CreateResponse(HttpStatusCode.UnprocessableEntity);
                return new MultiResponse()
                {
                    QueueMessages = null,
                    HttpResponse = errorResponse
                };
            }

            SaveMessageToBlobStorage(message);

            var response = req.CreateResponse();
            response.Headers.Add("Location", $@"http://localhost:7071/api/messages/{message.Id.ToString()}");
            await response.WriteAsJsonAsync(message, HttpStatusCode.Created).ConfigureAwait(false);
            var multiResponse = new MultiResponse()
            {
                QueueMessages = new[] { message },
                HttpResponse = response
            };

            return multiResponse;
        }

        private static void SaveMessageToBlobStorage(Message message)
        {
            var messageJson = JsonConvert.SerializeObject(message);

            // TODO: use IOC
            var connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
            var serviceClient = new BlobServiceClient(connectionString);
            var containerClient = serviceClient.GetBlobContainerClient("messages");
            containerClient.CreateIfNotExists();
            var blobClient = containerClient.GetBlobClient(message.Id.ToString());
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(messageJson)))
            {
                blobClient.Upload(stream);
            }
        }
    }
}
