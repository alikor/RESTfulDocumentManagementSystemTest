using System.Collections.Generic;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Documents.Models.HAL
{
    public class HalTemplate
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("method")]
        public string Method { get; set; }

        [JsonPropertyName("contentType")]
        public string ContentType { get; set; }

        [JsonPropertyName("properties")]
        public List<HalTemplateProperty> Properties { get; set; } = new List<HalTemplateProperty>();
    }


}