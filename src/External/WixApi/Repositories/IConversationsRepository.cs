namespace WixApi.Repositories
{
    public interface IConversationsRepository
    {
        Task<string> GetConversations();
    }
}
