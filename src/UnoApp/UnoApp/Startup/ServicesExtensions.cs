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
        
        // Add Shiny Mediator with standard configuration
        services.AddShinyMediator(cfg => 
        {
            cfg.UseUno();
            cfg.AddUnoPersistentCache();
        });
        
        // Register HTTP decorator
        services.AddSingleton<IHttpRequestDecorator, BearerAuthenticationHttpDecorator>();
    }
}
