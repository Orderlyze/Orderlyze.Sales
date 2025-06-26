using Refit;
using WixApi.Constants;

namespace WixApi.Repositories
{
    public abstract class BaseWixRepository<TApi>
    {
        protected readonly TApi RepositoryApi;

        protected BaseWixRepository()
        {
            RepositoryApi = RestService.For<TApi>(WixConstants.BaseUrl, BuildApiSettings());
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
            return Task.FromResult("Authorization: " + WixConstants.ApiKey);
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
