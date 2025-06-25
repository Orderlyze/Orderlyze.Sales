using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Kiota.Abstractions;
using MyApi.Client;
using MyApi.Client.Contacts.V4.Contacts;
using WixApi.Constants;
using WixApi.Mediator.Requests;
using WixApi.Models;

namespace WixApi.Mediator.Handlers
{
    [SingletonHandler]
    internal class ListContactsRequestHandler(ApiClient apiClient)
        : IRequestHandler<ListContactsRequest, ContactsGetResponse>
    {
        public Task<ContactsGetResponse> Handle(
            ListContactsRequest request,
            IMediatorContext context,
            CancellationToken cancellationToken
        )
        {
            return apiClient
                .Contacts.V4.Contacts.WithUrl(
                    $"{WixConstants.BaseUrl}/contacts/v4/contacts?sort.fieldName=updatedDate&paging.limit={request.Limit}"
                )
                .GetAsContactsGetResponseAsync();
        }
    }
}
