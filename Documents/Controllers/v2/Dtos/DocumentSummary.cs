using Documents.Models;
namespace Documents.Controllers.v2.Dtos
{
    public class DocumentSummary
    {
        public DocumentSummary()
        {
            
        }
        public DocumentSummary(Document document)
        {
            Id = document.Id;
            Title = document.Title;
        }

        public int Id { get; set; }
        public string Title { get; set; }
    }
}