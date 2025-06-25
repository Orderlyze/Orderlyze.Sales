using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Kiota.Abstractions;
using MyApi.Client;
using MyApi.Client.Contacts.V4.Contacts;
using WixApi.Mediator.Requests;
using WixApi.Models;

namespace WixApi.Mediator.Handlers
{
    [SingletonHandler]
    internal class ListContactsRequestHandler(ApiClient apiClient)
        : IRequestHandler<ListContactsRequest, List<WixContact>>
    {
        public async Task<List<WixContact>> Handle(
            ListContactsRequest request,
            IMediatorContext context,
            CancellationToken cancellationToken
        )
        {
            var stream = await apiClient.Contacts.V4.Contacts.GetAsContactsGetResponseAsync(x =>
            {
                x.QueryParameters.Limit = 10;
            });
            return null;
        }
    }
}
