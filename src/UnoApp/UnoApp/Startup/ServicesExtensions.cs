using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shiny.Extensions.DependencyInjection;
using Shiny.Mediator;
using UnoApp.Services.Common;
using WixApi;

namespace UnoApp.Startup;

internal static class ServicesExtensions
{
    internal static void AddCommonServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        // Add all services marked with [Service] attribute
        services.AddGeneratedServices();
        
        // Add startup tasks for initialization
        services.AddGeneratedStartupTasks();
        
        services.AddWixApi(configuration);
        services.AddShinyMediator(cfg => cfg.UseUno());
        
        // Register HttpClient
        services.AddHttpClient();
    }
}
