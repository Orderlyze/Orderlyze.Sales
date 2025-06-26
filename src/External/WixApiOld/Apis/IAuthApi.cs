using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Refit;
using WixApi.Models;

namespace WixApi.Apis
{
    public interface IAuthApi
    {
        [Post("/oauth/access")]
        Task<AuthResponse> Access([Body] AuthPayload authPayload);

        [Post("/oauth/access")]
        Task<AuthResponse> RefreshToken([Body] AuthRefreshPayload authRefreshPayload);
    }
}
