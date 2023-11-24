using System.Text.Json.Serialization;

namespace Documents.Models.HAL
{
    public class HalTemplateProperty
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("prompt")]
        public string Prompt { get; set; }

        [JsonPropertyName("required")]
        public bool Required { get; set; }
    }
}