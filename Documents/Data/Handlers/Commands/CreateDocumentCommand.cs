

namespace Documents.Data.Handlers.Commands

{
    public class CreateDocumentCommand
    {
        public CreateDocumentCommand(string title, string content)
        {
            Title = title;
            Content = content;
        }

        public string Title { get; set; }
        public string Content { get; set; }
    }
}