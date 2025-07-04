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
        var apiKey = wixApiSection["ApiKey"];
        var accountId = wixApiSection["AccountId"] ?? throw new InvalidOperationException("WixApi:AccountId is not configured");
        var siteId = wixApiSection["SiteId"] ?? throw new InvalidOperationException("WixApi:SiteId is not configured");

        // Check for API key in environment variable if not in config
        if (string.IsNullOrEmpty(apiKey))
        {
            apiKey = Environment.GetEnvironmentVariable("WixApi__ApiKey");
        }
        
        if (string.IsNullOrEmpty(apiKey))
        {
            throw new InvalidOperationException("WixApi:ApiKey is not configured. Please set it in appsettings.json, appsettings.Development.json, or as an environment variable (WixApi__ApiKey)");
        }

        // Log configuration for debugging (remove sensitive data in production)
        var logger = services.BuildServiceProvider().GetService<ILogger<IConfiguration>>();
        logger?.LogDebug($"WixApi configured with BaseUrl: {baseUrl}, AccountId: {accountId}, SiteId: {siteId}, ApiKey: {(string.IsNullOrEmpty(apiKey) ? "NOT SET" : "SET")}");

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
