namespace WixApi
{

    using Microsoft.Extensions.DependencyInjection;
    using WixApi.Repositories;

    public static class ModuleInitializer
    {
        public static IServiceCollection AddWixApi(this IServiceCollection services)
        {
            return services.AddTransient<IWixContactsRepository, WixContactsRepository>()
                .AddTransient<IConversationsRepository, ConversationsRepository>();
        }
    }
}