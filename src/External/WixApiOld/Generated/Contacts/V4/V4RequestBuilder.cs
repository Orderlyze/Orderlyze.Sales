// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions;
using MyApi.Client.Contacts.V4.Contacts;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System;
namespace MyApi.Client.Contacts.V4
{
    /// <summary>
    /// Builds and executes requests for operations under \contacts\v4
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class V4RequestBuilder : BaseRequestBuilder
    {
        /// <summary>The contacts property</summary>
        public global::MyApi.Client.Contacts.V4.Contacts.ContactsRequestBuilder Contacts
        {
            get => new global::MyApi.Client.Contacts.V4.Contacts.ContactsRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>
        /// Instantiates a new <see cref="global::MyApi.Client.Contacts.V4.V4RequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public V4RequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/contacts/v4", pathParameters)
        {
        }
        /// <summary>
        /// Instantiates a new <see cref="global::MyApi.Client.Contacts.V4.V4RequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public V4RequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/contacts/v4", rawUrl)
        {
        }
    }
}
#pragma warning restore CS0618
