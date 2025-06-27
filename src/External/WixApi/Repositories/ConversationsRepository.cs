using WixApi.Apis;
using Microsoft.Extensions.Options;

namespace WixApi.Repositories
{
    public class ConversationsRepository : BaseWixRepository<IConversationApi>, IConversationsRepository
    {
        public ConversationsRepository(IHttpClientFactory clientFactory, IOptions<WixApiOptions> options)
            : base(clientFactory, options)
        {
        }
        public async Task<string> GetConversations()
        {
            return await TryRequest(async () => await this.RepositoryApi.GetConversations("de945875-68e4-33e2-abc9-e71765366049"));
        }
    }
}
