using Documents.Data.Handlers.Commands;

namespace Documents.Data.Handlers
{
    public interface ICreateDocumentCommandHandler
    {
        Task<int> Handle(CreateDocumentCommand command);
    }
}