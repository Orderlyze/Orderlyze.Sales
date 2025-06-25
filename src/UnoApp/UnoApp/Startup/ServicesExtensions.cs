using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection.Extensions;
using UnoApp.Services.Common;

namespace UnoApp.Startup;

internal static class ServicesExtensions
{
    internal static void AddCommonServices(this IServiceCollection services)
    {
        services.TryAddSingleton<BaseServices>();
    }
}
