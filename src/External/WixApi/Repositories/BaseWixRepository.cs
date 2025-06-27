using Refit;
using Microsoft.Extensions.Options;

namespace WixApi.Repositories
{
    public abstract class BaseWixRepository<TApi>
    {
        protected readonly TApi RepositoryApi;
        private readonly IHttpClientFactory _clientFactory;
        private readonly WixApiOptions _options;

        protected BaseWixRepository(IHttpClientFactory clientFactory, IOptions<WixApiOptions> options)
        {
            _clientFactory = clientFactory;
            _options = options.Value;
            var client = _clientFactory.CreateClient("WixApiClient");
            client.BaseAddress = new Uri(_options.BaseUrl);
            RepositoryApi = RestService.For<TApi>(client, BuildApiSettings());
        }

        /// <summary>
        /// Build api settings
        /// </summary>
        /// <returns>Refit Settings</returns>
        protected virtual RefitSettings BuildApiSettings()
        {
            return new RefitSettings
            {
            };
        }

        /// <summary>
        /// Here we provide the Auth token for requests
        /// </summary>
        /// <returns>Current access token</returns>
        protected virtual Task<string> GetAuthorizationHeaderValueAsync(HttpRequestMessage message, CancellationToken token)
        {
            return Task.FromResult("Authorization: " + _options.ApiKey);
        }

        protected virtual async Task<T> TryRequest<T>(Func<Task<T>> func, T defaultValue = default(T))
        {
            try
            {
                return await func.Invoke();
            }
            catch (ApiException ex)
            {
                throw ex;
            }
        }


    }
}
