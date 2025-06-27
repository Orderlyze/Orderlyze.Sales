using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WixApi.Repositories;

namespace WixApi.Sample
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureHostConfiguration(options =>
                {
                    //options.AddWixApi();
                })
                .ConfigureServices(
                    (context, services) =>
                    {
                        services.AddWixApi(context.Configuration);
                    }
                )
                .Build();

            var myService = host.Services.GetRequiredService<IWixContactsRepository>();

            var test = await myService.GetContactsAsync();
            await host.RunAsync();
        }
    }
}
