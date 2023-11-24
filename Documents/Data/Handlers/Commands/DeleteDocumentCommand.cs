

namespace Documents.Data.Handlers.Commands

{
    public class DeleteDocumentCommand
    {
        public DeleteDocumentCommand(int id)
        {
            Id = id;
        }

        public int Id { get; set; }
    }
}