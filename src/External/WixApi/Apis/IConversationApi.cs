using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WixApi.Constants;

namespace WixApi.Apis
{
    [Headers($"Authorization: {WixConstants.ApiKey}",
        $"wix-account-id: {WixConstants.AccountId}",
        $"wix-site-id: {WixConstants.SiteId}"
        )]
    public interface IConversationApi
    {
        [Post("/inbox/v2/conversations")]
        Task<string> GetConversations([Query] string conversationId);
    }
}
