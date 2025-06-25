using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyApi.Client;
using MyApi.Client.Inbox.V2.Conversations;

namespace WixApi.Mediator.Requests
{
    public record ListConversationsRequest(string ContactId) : IRequest<ConversationsPostResponse>;
}
