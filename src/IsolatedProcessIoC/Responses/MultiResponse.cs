using IsolatedProcessIoC.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace IsolatedProcessIoC.Responses;

public class MultiResponse
{
    [QueueOutput("outqueue",Connection = "AzureWebJobsStorage")]
    public Message[]? Messages { get; set; }

    public HttpResponseData? HttpResponse { get; set; }
}
