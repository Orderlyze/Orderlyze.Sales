using System.Text.Json.Serialization;

namespace WixApi.Models
{
    public class Filter
    {
        [JsonPropertyName("info.emails.email")]
        public string InfoEmailsEmail { get; set; }
    }

    public class Query
    {
        [JsonPropertyName("filter")]
        public Filter Filter { get; set; }

        [JsonPropertyName("fieldsets")]
        public List<string> Fieldsets => new List<string>
        {
            "FULL"
        };
    }

    public class ContactQuery
    {
        [JsonPropertyName("query")]
        public Query Query { get; set; }
    }
}

