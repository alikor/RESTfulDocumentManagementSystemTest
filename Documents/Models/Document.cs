

using System;

namespace Documents.Models
{
    
    public class Document
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 23 + Id.GetHashCode();
            hash = hash * 23 + (Title != null ? Title.GetHashCode() : 0);
            hash = hash * 23 + (Content != null ? Content.GetHashCode() : 0);
            hash = hash * 23 + ModifiedDate.GetHashCode();
            hash = hash * 23 + ModifiedDate.GetHashCode();

            return hash;
        }
    }
}