using Documents.Data.Handlers.Commands;
using Documents.Models;
using Documents.Utitlies;

namespace Documents.Data.Handlers
{
    public class CreateDocumentCommandHandler: ICreateDocumentCommandHandler
    {
        private readonly IAppDbContext _context;
        private readonly IDateTimeProvider _dateTimeProvider;

        public CreateDocumentCommandHandler(IAppDbContext context, IDateTimeProvider dateTimeProvider)
        {
            _context = context;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<int> Handle(CreateDocumentCommand command)
        {
            var now = _dateTimeProvider.UtcNow;
            var document = new Document
            {
                Title = command.Title,
                Content = command.Content,
                CreatedDate = now,
                ModifiedDate = now
            };

            _context.Documents.Add(document);
            await _context.SaveChangesAsync();

            return document.Id;
        }
    }
}

