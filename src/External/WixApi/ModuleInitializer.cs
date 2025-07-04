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

        // API key MUST come from environment variable for security
        var apiKey = configuration["WixApi:ApiKey"]; // This will pick up environment variables due to configuration setup
        
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

        // Log configuration for debugging
        var logger = services.BuildServiceProvider().GetService<ILogger<IConfiguration>>();
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
