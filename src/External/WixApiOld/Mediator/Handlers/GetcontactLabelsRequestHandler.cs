using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kiota.Abstractions;
using MyApi.Client;
using WixApi.Mediator.Requests;

namespace WixApi.Mediator.Handlers
{
    [SingletonHandler]
    internal class GetcontactLabelsRequestHandler(ApiClient apiClient)
        : IRequestHandler<GetContactLabelsRequest, object>
    {
        public async Task<object> Handle(
            GetContactLabelsRequest request,
            IMediatorContext context,
            CancellationToken cancellationToken
        )
        {
            await apiClient
                .Contacts.V4.Contacts[request.ContactId]
                .Labels.PostAsync(new MyApi.Client.Models.LabelContactRequest());
            return default;
        }
    }
}
