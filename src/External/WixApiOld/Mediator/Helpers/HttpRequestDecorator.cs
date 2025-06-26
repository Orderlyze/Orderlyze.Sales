using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Shiny.Mediator.Http;
using WixApi.Constants;

namespace WixApi.Mediator.Helpers
{
    internal class HttpRequestDecorator : IHttpRequestDecorator
    {
        public Task Decorate(HttpRequestMessage httpMessage, IMediatorContext context)
        {
            httpMessage.Headers.Authorization = new AuthenticationHeaderValue(
                "Authorization",
                WixConstants.ApiKey
            );
            return Task.CompletedTask;
        }
    }
}
