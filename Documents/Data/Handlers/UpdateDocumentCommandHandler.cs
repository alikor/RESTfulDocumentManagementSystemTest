using Documents.Data.Handlers.Commands;
using Documents.Models;
using Documents.Utitlies;

namespace Documents.Data.Handlers
{
    public class UpdateDocumentCommandHandler: IUpdateDocumentCommandHandler
    {
        private readonly IAppDbContext _context;
        private readonly IDateTimeProvider _dateTimeProvider;

        public UpdateDocumentCommandHandler(IAppDbContext context, IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
            _context = context;
        }

        public async Task Handle(UpdateDocumentCommand command)
        {
            var doc = await _context.Documents.FindAsync(command.Id);

            doc.Title = command.Title;
            doc.Content = command.Content;
            doc.ModifiedDate = _dateTimeProvider.UtcNow;

            await _context.SaveChangesAsync();
        }
    }
}

