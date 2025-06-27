using Refit;

namespace WixApi.Apis
{
    public interface IConversationApi
    {
        [Post("/inbox/v2/conversations")]
        Task<string> GetConversations([Query] string conversationId);
    }
}
