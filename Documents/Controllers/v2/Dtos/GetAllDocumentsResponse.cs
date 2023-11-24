using Documents.Models;

using Documents.Data.Quries;
using System.Text.Json.Serialization; // Add missing import statement
namespace Documents.Controllers.v2.Dtos
{

    public class GetAllDocumentsResponse
    {
        public GetAllDocumentsResponse() : this(new PagedResult<Document>())
        {
            
        }
        public GetAllDocumentsResponse(PagedResult<Document> pageResult)
        {
            TotalCount = pageResult.TotalCount;
            PageNumber = pageResult.PageNumber;
            PageSize = pageResult.PageSize;
        }

        [JsonPropertyName("totalCount")]
        public int TotalCount { get; set; }

         [JsonPropertyName("pageNumber")]
        public int PageNumber { get; set; }

         [JsonPropertyName("pageSize")]
        public int PageSize { get; set; }
    }
}