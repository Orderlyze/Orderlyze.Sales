// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions;
using MyApi.Client.Forms.V2.Submissions;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System;
namespace MyApi.Client.Forms.V2
{
    /// <summary>
    /// Builds and executes requests for operations under \forms\v2
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class V2RequestBuilder : BaseRequestBuilder
    {
        /// <summary>The submissions property</summary>
        public global::MyApi.Client.Forms.V2.Submissions.SubmissionsRequestBuilder Submissions
        {
            get => new global::MyApi.Client.Forms.V2.Submissions.SubmissionsRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>
        /// Instantiates a new <see cref="global::MyApi.Client.Forms.V2.V2RequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public V2RequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/forms/v2", pathParameters)
        {
        }
        /// <summary>
        /// Instantiates a new <see cref="global::MyApi.Client.Forms.V2.V2RequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public V2RequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/forms/v2", rawUrl)
        {
        }
    }
}
#pragma warning restore CS0618
