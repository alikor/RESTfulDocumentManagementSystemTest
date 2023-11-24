using Documents.Models;

namespace Documents.Data.Quries
{
    public class DocumentsQueryBuilder : IDocumentsQueryBuilder
    {
        private readonly IAppDbContext _context;

        public DocumentsQueryBuilder(IAppDbContext context)
        {
            _context = context;
        }

    public PagedResult<Document> Build(int pageNumber, int pageSize)
    {
        var totalCount = _context.Documents.Count();
        var documents = _context.Documents
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize).ToList();

        return new PagedResult<Document>
        {
            Documents = documents,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

        public Document BuildById(int id)
        {
            return _context.Documents.FirstOrDefault(d => d.Id == id);
        }
    }
}