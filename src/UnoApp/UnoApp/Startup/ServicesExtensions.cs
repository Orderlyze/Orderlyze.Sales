using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shiny.Extensions.DependencyInjection;
using Shiny.Mediator;
using Shiny.Mediator.Http;
using Authentication = UnoApp.Services.Authentication;
using UnoApp.Services.Common;
using UnoApp.Services.Http;
using WixApi;
using UnoApp;

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
        
        // Register Authentication Service
        services.AddSingleton<Authentication.IAuthenticationService, Authentication.AuthenticationService>();
        
        // Add Shiny Mediator without HTTP client (to avoid conflicts with non-HTTP requests)
        services.AddShinyMediator(cfg => 
        {
            cfg.UseUno();
            cfg.AddUnoPersistentCache();
            cfg.PreventEventExceptions();
            cfg.AddTimerRefreshStreamMiddleware();
        }, includeStandardMiddleware: false);
        
        // Register generated handlers
        services.AddDiscoveredMediatorHandlersFromUnoApp();
        
        // Manually register HTTP handlers for generated HTTP requests
        services.AddScoped(typeof(IRequestHandler<,>), typeof(HttpRequestHandler<,>));
        services.AddSingleton<IRequestHandler<HttpDirectRequest, object?>, HttpDirectRequestHandler>();
        
        // Register HTTP decorator
        services.AddSingleton<IHttpRequestDecorator, BearerAuthenticationHttpDecorator>();
    }
}
