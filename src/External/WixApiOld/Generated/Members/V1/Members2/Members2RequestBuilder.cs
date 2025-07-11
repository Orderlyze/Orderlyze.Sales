// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions;
using MyApi.Client.Members.V1.Members2.Item;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System;
namespace MyApi.Client.Members.V1.Members2
{
    /// <summary>
    /// Builds and executes requests for operations under \members\v1\members2
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class Members2RequestBuilder : BaseRequestBuilder
    {
        /// <summary>Gets an item from the MyApi.Client.members.v1.members2.item collection</summary>
        /// <param name="position">Member ID</param>
        /// <returns>A <see cref="global::MyApi.Client.Members.V1.Members2.Item.WithMemberItemRequestBuilder"/></returns>
        public global::MyApi.Client.Members.V1.Members2.Item.WithMemberItemRequestBuilder this[string position]
        {
            get
            {
                var urlTplParams = new Dictionary<string, object>(PathParameters);
                urlTplParams.Add("memberId", position);
                return new global::MyApi.Client.Members.V1.Members2.Item.WithMemberItemRequestBuilder(urlTplParams, RequestAdapter);
            }
        }
        /// <summary>
        /// Instantiates a new <see cref="global::MyApi.Client.Members.V1.Members2.Members2RequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public Members2RequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/members/v1/members2", pathParameters)
        {
        }
        /// <summary>
        /// Instantiates a new <see cref="global::MyApi.Client.Members.V1.Members2.Members2RequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public Members2RequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/members/v1/members2", rawUrl)
        {
        }
    }
}
#pragma warning restore CS0618
