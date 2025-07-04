using Microsoft.Extensions.Configuration;
using UnoApp.Services.Configuration;

namespace UnoApp.Startup;

internal static class HostBuilderExtensions
{
    internal static IHostBuilder AddLogging(this IHostBuilder hostBuilder)
    {
        return hostBuilder.UseLogging(
            configure: (context, logBuilder) =>
            {
                // Configure log levels for different categories of logging
                logBuilder
                    .SetMinimumLevel(
                        context.HostingEnvironment.IsDevelopment()
                            ? LogLevel.Information
                            : LogLevel.Warning
                    )
                    // Default filters for core Uno Platform namespaces
                    .CoreLogLevel(LogLevel.Warning);

                // Uno Platform namespace filter groups
                // Uncomment individual methods to see more detailed logging
                //// Generic Xaml events
                //logBuilder.XamlLogLevel(LogLevel.Debug);
                //// Layout specific messages
                //logBuilder.XamlLayoutLogLevel(LogLevel.Debug);
                //// Storage messages
                //logBuilder.StorageLogLevel(LogLevel.Debug);
                //// Binding related messages
                //logBuilder.XamlBindingLogLevel(LogLevel.Debug);
                //// Binder memory references tracking
                //logBuilder.BinderMemoryReferenceLogLevel(LogLevel.Debug);
                //// DevServer and HotReload related
                //logBuilder.HotReloadCoreLogLevel(LogLevel.Information);
                //// Debug JS interop
                //logBuilder.WebAssemblyLogLevel(LogLevel.Debug);
            },
            enableUnoLogging: true
        );
    }

    internal static IHostBuilder AddConfig(this IHostBuilder hostBuilder)
    {
        return hostBuilder
            .UseConfiguration(configure: configBuilder =>
                configBuilder
                    .EmbeddedSource<App>()
                    .Section<AppConfig>()
            )
            .ConfigureAppConfiguration((context, configBuilder) =>
            {
                // Add environment variables to app configuration
                configBuilder.AddEnvironmentVariables();
            });
    }

    internal static IHostBuilder AddEnvironment(this IHostBuilder hostBuilder)
    {
        // Load environment variables from .env file if present
        DotEnvLoader.Load();
        
#if DEBUG
        // Switch to Development environment when running in DEBUG
        hostBuilder = hostBuilder.UseEnvironment(Environments.Development);
#endif
        return hostBuilder;
    }

    internal static IHostBuilder AddLocalization(this IHostBuilder hostBuilder)
    {
        return hostBuilder.UseLocalization();
    }

    internal static IHostBuilder AddSerialization(this IHostBuilder hostBuilder)
    {
        return hostBuilder.UseSerialization(
            (context, services) =>
                services
                    .AddContentSerializer(context)
                    .AddJsonTypeInfo(WeatherForecastContext.Default.IImmutableListWeatherForecast)
        );
    }

    internal static IHostBuilder AddHttp(this IHostBuilder hostBuilder)
    {
        return hostBuilder.UseHttp(
            (context, services) =>
            {
#if DEBUG
                // DelegatingHandler will be automatically injected
                services.AddTransient<DelegatingHandler, DebugHttpHandler>();
#endif
                services.AddSingleton<IWeatherCache, WeatherCache>();
                services.AddKiotaClient<WeatherServiceClient>(
                    context,
                    options: new EndpointOptions { Url = context.Configuration["ApiClient:Url"]! }
                );
            }
        );
    }
}
