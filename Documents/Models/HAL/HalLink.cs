namespace Documents.Models.HAL
{
    public class HalLink
    {
        public HalLink() : this("")    
        {
        }

        public HalLink(string href)
        {
            this.Href = href;
        }
        
        
        public string Href { get; set; }
        public string Method { get; set; }
        public bool? Templated { get; set; }
        public string Type { get; set; }
        public string Deprecation { get; set; }
        public string Name { get; set; }
        public string Profile { get; set; }
        public string Title { get; set; }
        public string Hreflang { get; set; }
    }
}
