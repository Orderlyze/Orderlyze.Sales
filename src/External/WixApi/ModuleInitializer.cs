namespace WixApi;

using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WixApi.Repositories;

public static class ModuleInitializer
{
    public static IServiceCollection AddWixApi(this IServiceCollection services, IConfiguration configuration)
    {
        var wixApiSection = configuration.GetSection("WixApi");
        var baseUrl = wixApiSection["BaseUrl"] ?? throw new InvalidOperationException("WixApi:BaseUrl is not configured");
        var accountId = wixApiSection["AccountId"] ?? throw new InvalidOperationException("WixApi:AccountId is not configured");
        var siteId = wixApiSection["SiteId"] ?? throw new InvalidOperationException("WixApi:SiteId is not configured");

        // Get logger for debugging
        var logger = services.BuildServiceProvider().GetService<ILogger<IConfiguration>>();

        // API key handling
        var apiKey = configuration["WixApi:ApiKey"]; // This will pick up environment variables due to configuration setup
        
#if !__WASM__
        if (string.IsNullOrEmpty(apiKey))
        {
            // Also try direct environment variable access as fallback
            apiKey = Environment.GetEnvironmentVariable("WixApi__ApiKey") ?? 
                     Environment.GetEnvironmentVariable("WIXAPI__APIKEY");
        }
        
        if (string.IsNullOrEmpty(apiKey))
        {
            throw new InvalidOperationException(
                "WixApi:ApiKey is not configured. Please set the environment variable WixApi__ApiKey. " +
                "For local development, you can use a .env file or set the environment variable directly.");
        }
#else
        // In WebAssembly, we need to handle API key differently
        // For development, we can use a placeholder or require it to be set in appsettings
        if (string.IsNullOrEmpty(apiKey))
        {
            // In WASM, we might want to use a development key or handle authentication differently
            logger?.LogWarning("WixApi:ApiKey is not configured in WebAssembly. WixApi functionality will be limited.");
            apiKey = "WASM_PLACEHOLDER_KEY"; // This will need proper handling in production
        }
#endif

        // Log configuration for debugging
        logger?.LogInformation($"WixApi configured with BaseUrl: {baseUrl}, AccountId: {accountId}, SiteId: {siteId}, ApiKey is {(string.IsNullOrEmpty(apiKey) ? "NOT SET" : "SET")}");

        services.AddHttpClient("WixApiClient", client =>
        {
            client.BaseAddress = new Uri(baseUrl);
            // Wix API expects the API key directly in the Authorization header (no Bearer prefix)
            client.DefaultRequestHeaders.Add("Authorization", apiKey);
            client.DefaultRequestHeaders.Add("wix-account-id", accountId);
            client.DefaultRequestHeaders.Add("wix-site-id", siteId);
        });

        return services
            .AddTransient<IWixContactsRepository, WixContactsRepository>()
            .AddTransient<IConversationsRepository, ConversationsRepository>();
    }
}
