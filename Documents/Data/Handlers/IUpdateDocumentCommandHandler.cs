using Documents.Data.Handlers.Commands;

namespace Documents.Data.Handlers
{

    public interface IUpdateDocumentCommandHandler
    {
        Task Handle(UpdateDocumentCommand command);
    }

}