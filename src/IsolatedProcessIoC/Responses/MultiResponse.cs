using IsolatedProcessIoC.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace IsolatedProcessIoC.Responses;

public class MultiResponse
{
    [QueueOutput("outqueue",Connection = "AzureWebJobsStorage")]
    public Message[]? QueueMessages { get; set; }

    // at the moment it is not possible to control the blob name. Using the Blob SDK instead
    // https://github.com/Azure/azure-functions-python-worker/issues/838
    // https://github.com/Azure/azure-functions-python-worker/issues/652
    // [BlobOutput("azure-webjobs-hosts/messages", Connection = "AzureWebJobsStorage" )]
    // public string? Message { get; set; }

    public HttpResponseData? HttpResponse { get; set; }
}
