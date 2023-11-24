using Documents.Data.Handlers.Commands;
using Documents.Models;

namespace Documents.Data.Handlers
{
    public class DeleteDocumentCommandHandler: IDeleteDocumentCommandHandler
    {
        private readonly IAppDbContext _context;

        public DeleteDocumentCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task Handle(DeleteDocumentCommand command)
        {
            var doc = new Document { Id = command.Id };
            _context.Documents.Attach(doc);
            _context.Documents.Remove(doc);
            
            await _context.SaveChangesAsync();
        }
    }
}

