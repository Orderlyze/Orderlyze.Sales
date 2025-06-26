using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnoApp.Extensions;

internal static class NavigationExtensions
{
    public static Task<NavigationResponse?> NavigateRegionAsync(
        this INavigator navigator,
        object sender,
        string region,
        string view,
        string preRoute = "./",
        object? data = null,
        CancellationToken cancellation = default
    )
    {
        return navigator.NavigateRouteAsync(
            navigator,
            $"{preRoute}{region}/{view}",
            data: data,
            cancellation: cancellation
        );
    }
}
