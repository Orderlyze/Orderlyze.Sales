namespace WixApi;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using WixApi.Repositories;

public static class ModuleInitializer
{
    public static IServiceCollection AddWixApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<WixApiOptions>(configuration.GetSection("WixApi"));

        services.AddHttpClient("WixApiClient", (sp, client) =>
        {
            var options = sp.GetRequiredService<IOptions<WixApiOptions>>().Value;
            client.BaseAddress = new Uri(options.BaseUrl);
            client.DefaultRequestHeaders.Add("Authorization", options.ApiKey);
            client.DefaultRequestHeaders.Add("wix-account-id", options.AccountId);
            client.DefaultRequestHeaders.Add("wix-site-id", options.SiteId);
        });

        return services
            .AddTransient<IWixContactsRepository, WixContactsRepository>()
            .AddTransient<IConversationsRepository, ConversationsRepository>();
    }
}
