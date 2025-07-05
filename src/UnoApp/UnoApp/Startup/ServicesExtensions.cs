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
        services.TryAddScoped<BaseServices>();
        // Add all services marked with [Service] attribute
        services.AddGeneratedServices();
        
        
        services.AddWixApi(configuration);
        
        // Register HttpClient
        services.AddHttpClient();
        
        // Add Shiny Mediator without standard middleware to avoid automatic HTTP handler registration
        services.AddShinyMediator(cfg =>
        {
            cfg.UseUno();
            cfg.PreventEventExceptions();
            cfg.AddTimerRefreshStreamMiddleware();
            // Note: We're not calling AddHttpClient() to avoid registering the generic HttpRequestHandler
        }, includeStandardMiddleware: false);
    }
}
