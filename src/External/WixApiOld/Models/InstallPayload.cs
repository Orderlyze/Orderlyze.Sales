using System;
using System.Collections.Generic;
using System.Text;
using WixApi.Constants;

namespace WixApi.Models
{
    public class InstallPayload
    {
        public string token => AuthConstants.Token;
        public string appId => AuthConstants.AppId;
        public string redirectUrl => AuthConstants.RedirectUri;
    }
}
