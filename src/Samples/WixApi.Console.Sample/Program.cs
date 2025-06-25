using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WixApi.Constants;
using WixApi.Mediator.Requests;

namespace WixApi.Sample
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureHostConfiguration(options =>
                {
                    options.AddWixApi();
                })
                .ConfigureServices(
                    (context, services) =>
                    {
                        services.AddWixApi();
                    }
                )
                .Build();

            var myService = host.Services.GetRequiredService<IMediator>();
            var request = new ListContactsRequest { };

            var result = await myService.Request(request);
            await host.RunAsync();
        }
    }
}
