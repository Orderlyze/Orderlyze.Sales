using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using MyApi.Client;
using Shiny.Mediator.Http;
using WixApi.Helpers.Kiota;
using WixApi.Mediator.Helpers;

namespace WixApi
{
    public static class Initializer
    {
        public static void AddWixApi(this IConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.AddJsonStream(
                Assembly
                    .GetExecutingAssembly()
                    .GetManifestResourceStream("WixApi.mediatorconfig.json")!
            );
        }

        public static void AddWixApi(this IServiceCollection services)
        {
            services.AddDiscoveredMediatorHandlersFromWixApi();
            services.AddAdapter();
            services.AddShinyMediator(options => { });
        }

        public static void AddAdapter(this IServiceCollection services)
        {
            // 1. Eigene Header-Middleware registrieren
            services.AddTransient<WixHeadersHandler>();

            // 3. HttpClient + Pipeline konfigurieren
            services
                .AddHttpClient(
                    "WixKiotaClient",
                    client =>
                    {
                        client.BaseAddress = new Uri("https://www.wixapis.com/"); // oder dein tatsächlicher API-Endpunkt
                    }
                )
                .AddHttpMessageHandler<WixHeadersHandler>();

            // 4. Registriere Kiota Adapter + Client mit konfiguriertem HttpClient
            services.AddSingleton<ApiClient>(sp =>
            {
                var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient("WixKiotaClient");

                var authProvider = new ApiKeyAuthenticationProvider(
                    Constants.WixConstants.ApiKey,
                    "Authorization",
                    ApiKeyAuthenticationProvider.KeyLocation.Header
                );

                var adapter = new HttpClientRequestAdapter(authProvider, httpClient: httpClient)
                {
                    BaseUrl = "https://www.wixapis.com/", // Optional, falls nicht im Client gesetzt
                };

                return new ApiClient(adapter); // ersetzt "client"
            });
        }
    }
}
