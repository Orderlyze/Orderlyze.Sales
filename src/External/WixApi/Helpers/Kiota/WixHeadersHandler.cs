using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WixApi.Constants;

namespace WixApi.Helpers.Kiota
{
    public class WixHeadersHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken
        )
        {
            // Header nur hinzufügen, falls nicht bereits vorhanden (Vermeidung von Duplikaten bei Retries)
            if (!request.Headers.Contains("wix-account-id"))
                request.Headers.Add("wix-account-id", WixConstants.AccountId);
            if (!request.Headers.Contains("wix-site-id"))
                request.Headers.Add("wix-site-id", WixConstants.SiteId);

            request.Headers.Remove("Accept");

            // Weiter zum nächsten Handler (oder zum HttpClient senden)
            var result = await base.SendAsync(request, cancellationToken);
            return result;
        }
    }
}
