using Documents.Data.Handlers.Commands;

namespace Documents.Data.Handlers
{

    public interface IDeleteDocumentCommandHandler
    {
        Task Handle(DeleteDocumentCommand command);
    }

}