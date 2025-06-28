namespace WixApi;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WixApi.Repositories;

public static class ModuleInitializer
{
    public static IServiceCollection AddWixApi(this IServiceCollection services, IConfiguration configuration)
    {
        var wixApiSection = configuration.GetSection("WixApi");
        var baseUrl = wixApiSection["BaseUrl"];
        var apiKey = wixApiSection["ApiKey"];
        var accountId = wixApiSection["AccountId"];
        var siteId = wixApiSection["SiteId"];

        services.AddHttpClient("WixApiClient", client =>
        {
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Add("Authorization", apiKey);
            client.DefaultRequestHeaders.Add("wix-account-id", accountId);
            client.DefaultRequestHeaders.Add("wix-site-id", siteId);
        });

        return services
            .AddTransient<IWixContactsRepository, WixContactsRepository>()
            .AddTransient<IConversationsRepository, ConversationsRepository>();
    }
}
