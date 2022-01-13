using IsolatedProcessIoC.Configuration;
using IsolatedProcessIoC.Handlers;
using Microsoft.Extensions.Hosting;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace IsolatedProcessIoC
{
    public class Program
    {
        public static void Main()
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults(worker => worker.UseNewtonsoftJson())
                .ConfigureOpenApi()
                .ConfigureServices(services =>
                {
                    services.AddTransient<IMessageHandler, MessageHandler>();
                    services.AddTransient<IFunctionConfiguration, FunctionConfiguration>();
                })
                .Build();

            host.Run();
        }
    }
}
