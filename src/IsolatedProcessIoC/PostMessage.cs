using System.Net;
using System.Threading.Tasks;
using IsolatedProcessIoC.Handlers;
using IsolatedProcessIoC.Models;
using IsolatedProcessIoC.Responses;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;

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
                    Messages = null,
                    HttpResponse = errorResponse
                };
            }

            var response = req.CreateResponse();
            // TODO: add location header
            await response.WriteAsJsonAsync(message, HttpStatusCode.Created).ConfigureAwait(false);

            var multiResponse = new MultiResponse()
            {
                Messages = new[] { message },
                HttpResponse = response
            };

            return multiResponse;
        }
    }
}
