using Documents.Data.Quries;
using Documents.Models;

public interface IDocumentsQueryBuilder
{
    PagedResult<Document> Build(int pageNumber, int pageSize);
    Document BuildById(int id);
}