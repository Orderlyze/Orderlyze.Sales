using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
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
        services.AddWixApi(configuration);
        services.AddShinyMediator(cfg => cfg.UseUno());
        services.TryAddScoped<BaseServices>();
    }
}
