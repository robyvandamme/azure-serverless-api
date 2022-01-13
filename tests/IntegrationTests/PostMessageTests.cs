using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace IntegrationTests;

public class PostMessageTests
{
    [Fact]
    public async Task NewMessage_With_Content_Returns_201()
    {
        var client = new HttpClient();

        // https://github.com/Azure/azure-functions-dotnet-worker/issues/374
        // var result = await client.PostAsJsonAsync("http://localhost:7071/api/messages", newMessage);

        var newMessage = new NewMessage();
        newMessage.Content = "Some content";

        var request = new HttpRequestMessage()
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(@TestConfiguration.FunctionUrl)
        };

        request.Content = new StringContent(JsonConvert.SerializeObject(newMessage), Encoding.UTF8, "application/json");
        var result = await client.SendAsync(request);

        result.StatusCode.Should().Be(HttpStatusCode.Created);
        result.Headers.Location.Should().NotBeNull();
        result.Headers.Location?.ToString().Should().StartWith(TestConfiguration.FunctionUrl);
    }

    [Fact]
    public async Task NewMessage_Without_Content_Returns_422()
    {
        var client = new HttpClient();

        // https://github.com/Azure/azure-functions-dotnet-worker/issues/374
        // var result = await client.PostAsJsonAsync("http://localhost:7071/api/messages", newMessage);

        var newMessage = new NewMessage();

        var request = new HttpRequestMessage()
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(@TestConfiguration.FunctionUrl)
        };

        request.Content = new StringContent(JsonConvert.SerializeObject(newMessage), Encoding.UTF8, "application/json");
        var result = await client.SendAsync(request);

        result.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
    }
}
