using System.Collections.Generic;
using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace Documents.Models.HAL
{
    public class HalResource<T>
    {
        public HalResource()
        {

        }
        public HalResource(T data)
        {
            Data = data;
        }

        public T Data { get; set; }

        [JsonPropertyName("_links")]
        public Dictionary<string, HalLink> Links { get; set; } = new Dictionary<string, HalLink>();

        [JsonPropertyName("_embedded")]
        public Dictionary<string, object> Embedded { get; set; } = new Dictionary<string, object>();

        public bool ShouldSerializeEmbedded()
        {
            return Embedded != null && Embedded.Count > 0;
        }

        [JsonPropertyName("_templates")]
        public Dictionary<string, HalTemplate> Templates { get; set; } = new Dictionary<string, HalTemplate>();

        public bool ShouldSerializeTemplates()
        {
            return Templates != null && Templates.Count > 0;
        }
    }
}