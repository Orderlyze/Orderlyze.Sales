using System;
using System.Collections.Generic;
using System.Text;

namespace WixApi.Models
{
    public class AuthPayload
    {
        /// <summary>
        /// Value must be set to "authorization_code"
        /// </summary>
        public string grant_type { get; set; }
        /// <summary>
        /// The App ID as defined in the Wix Developers Center
        /// </summary>
        public string client_id { get; set; }
        /// <summary>
        /// The Secret Key for your app as defined in your Wix Developers Center
        /// </summary>
        public string client_secret { get; set; }
        /// <summary>
        /// The authorization code received from us.
        /// </summary>
        public string code { get; set; }
    }
}
