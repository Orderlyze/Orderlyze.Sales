// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using MyApi.Client.Models;
using System.Collections.Generic;
using System.IO;
using System;
namespace MyApi.Client.FormSubmissionService.V4.Submissions.Query
{
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    #pragma warning disable CS1591
    public partial class QueryPostResponse : IAdditionalDataHolder, IParsable
    #pragma warning restore CS1591
    {
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>The paging property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::MyApi.Client.FormSubmissionService.V4.Submissions.Query.QueryPostResponse_paging? Paging { get; set; }
#nullable restore
#else
        public global::MyApi.Client.FormSubmissionService.V4.Submissions.Query.QueryPostResponse_paging Paging { get; set; }
#endif
        /// <summary>The submissions property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<global::MyApi.Client.Models.FormSubmission>? Submissions { get; set; }
#nullable restore
#else
        public List<global::MyApi.Client.Models.FormSubmission> Submissions { get; set; }
#endif
        /// <summary>
        /// Instantiates a new <see cref="global::MyApi.Client.FormSubmissionService.V4.Submissions.Query.QueryPostResponse"/> and sets the default values.
        /// </summary>
        public QueryPostResponse()
        {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::MyApi.Client.FormSubmissionService.V4.Submissions.Query.QueryPostResponse"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::MyApi.Client.FormSubmissionService.V4.Submissions.Query.QueryPostResponse CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::MyApi.Client.FormSubmissionService.V4.Submissions.Query.QueryPostResponse();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>
            {
                { "paging", n => { Paging = n.GetObjectValue<global::MyApi.Client.FormSubmissionService.V4.Submissions.Query.QueryPostResponse_paging>(global::MyApi.Client.FormSubmissionService.V4.Submissions.Query.QueryPostResponse_paging.CreateFromDiscriminatorValue); } },
                { "submissions", n => { Submissions = n.GetCollectionOfObjectValues<global::MyApi.Client.Models.FormSubmission>(global::MyApi.Client.Models.FormSubmission.CreateFromDiscriminatorValue)?.AsList(); } },
            };
        }
        /// <summary>
        /// Serializes information the current object
        /// </summary>
        /// <param name="writer">Serialization writer to use to serialize this model</param>
        public virtual void Serialize(ISerializationWriter writer)
        {
            _ = writer ?? throw new ArgumentNullException(nameof(writer));
            writer.WriteObjectValue<global::MyApi.Client.FormSubmissionService.V4.Submissions.Query.QueryPostResponse_paging>("paging", Paging);
            writer.WriteCollectionOfObjectValues<global::MyApi.Client.Models.FormSubmission>("submissions", Submissions);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}
#pragma warning restore CS0618
