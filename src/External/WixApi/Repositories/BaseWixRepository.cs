using Refit;


namespace WixApi.Repositories
{
    public abstract class BaseWixRepository<TApi>
    {
        protected readonly TApi RepositoryApi;
        private readonly IHttpClientFactory _clientFactory;

        protected BaseWixRepository(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            var client = _clientFactory.CreateClient("WixApiClient");
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
            return Task.FromResult(string.Empty);
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
