using Microsoft.Extensions.Configuration;

namespace IntegrationTests;

public class TestConfiguration
{
    static TestConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .AddJsonFile("testsettings.json", false);
        Configuration = builder.Build();

        FunctionUrl = Configuration["FunctionUrl"];
    }

    public static string FunctionUrl { get; set; }

    public static IConfigurationRoot Configuration { get; }
}
