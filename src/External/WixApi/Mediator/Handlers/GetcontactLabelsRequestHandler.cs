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
    internal class GetcontactLabelsRequestHandler(ApiClient apiClient)
        : IRequestHandler<GetContactLabelsRequest, object>
    {
        public Task<object> Handle(
            GetContactLabelsRequest request,
            IMediatorContext context,
            CancellationToken cancellationToken
        )
        {
            var requestInfo = new RequestInformation
            {
                HttpMethod = Method.POST,
                UrlTemplate = "{+baseurl}/contacts/v4/contacts/{contactId}/labels",
                PathParameters = new Dictionary<string, object>
                {
                    { "baseurl", "https://www.wixapis.com" },
                    { "contactId", "123456" },
                },
            };

            requestInfo.Headers.Add("Accept", "application/json");
            requestInfo.SetContentFromParsable(
                requestAdapter,
                "application/json",
                new { labels = new[] { "VIP", "newsletter" } }
            );

            // Aufruf:
            await requestAdapter.SendNoContentAsync(requestInfo);
        }
    }
}
