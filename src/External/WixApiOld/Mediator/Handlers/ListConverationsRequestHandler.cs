using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyApi.Client;
using MyApi.Client.Inbox.V2.Conversations;
using WixApi.Mediator.Requests;

namespace WixApi.Mediator.Handlers
{
    [SingletonHandler]
    internal class ListConverationsRequestHandler(ApiClient apiClient)
        : IRequestHandler<ListConversationsRequest, ConversationsPostResponse>
    {
        public Task<ConversationsPostResponse> Handle(
            ListConversationsRequest request,
            IMediatorContext context,
            CancellationToken cancellationToken
        )
        {
            return apiClient.Inbox.V2.Conversations.PostAsConversationsPostResponseAsync(
                new ConversationsPostRequestBody()
                {
                    ParticipantId = new ConversationsPostRequestBody_participantId()
                    {
                        ContactId = request.ContactId,
                    },
                }
            );
        }
    }
}
