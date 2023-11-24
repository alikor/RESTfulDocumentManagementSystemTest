using Microsoft.AspNetCore.Mvc;
using Documents.Models;
using Documents.Models.HAL;
using System.Text.Json;
using System.Text.Json.Serialization;
using Documents.Controllers.v2.Dtos;
using System.Reflection;
using System.Text.Encodings.Web;
using Documents.Data.Quries;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;

namespace Documents.Controllers.v2
{
    [Route("/v2")]
    [ApiController]
    [Authorize]
    public class GetDocumentsController : ControllerBase
    {
        private readonly IDocumentsQueryBuilder _documentsQueryBuilder;

        public GetDocumentsController(IDocumentsQueryBuilder documentsQueryBuilder)
        {
            _documentsQueryBuilder = documentsQueryBuilder;
        }

        [HttpGet]
        public IActionResult GetAllDocuments(int page = 1, int pageSize = 10)
        {
            ValidateRequest(pageSize);

            var documentsResults = _documentsQueryBuilder.Build(page, pageSize);

            var halResource = CreateHalResource(documentsResults);
            AddLinks(page, pageSize, documentsResults, halResource);
            AddEmbedded(documentsResults, halResource);
            AddTemplates(halResource);

            return Ok(halResourceToJson(halResource));
        }

        private static void ValidateRequest(int pageSize)
        {
            if (pageSize > 100)
            {
                throw new BadHttpRequestException("Page size cannot exceed 100.");
            }
        }

        private static HalResource<GetAllDocumentsResponse> CreateHalResource(PagedResult<Document> documentsResults)
        {
            return new HalResource<GetAllDocumentsResponse>(new GetAllDocumentsResponse(documentsResults));
        }

        private static void AddTemplates(HalResource<GetAllDocumentsResponse> halResource)
        {
            halResource.Templates = new Dictionary<string, HalTemplate>() { { "default", GetDocumentTemplate("create", "POST") } };	
        }

        private static void AddEmbedded(PagedResult<Document> documentsResults, HalResource<GetAllDocumentsResponse> halResource)
        {
            var embededDocuments = documentsResults.Documents.Select(d => new HalResource<DocumentSummary>(new DocumentSummary(d)) { Links = new Dictionary<string, HalLink>() { { "self", new HalLink($"/v2/{d.Id}") } }}).ToList();
            halResource.Embedded.Add("documents", embededDocuments);
        }

        private static void AddLinks(int page, int pageSize, PagedResult<Document> documentsResults, HalResource<GetAllDocumentsResponse> halResource)
        {
            halResource.Links.Add("self", new HalLink($"/v2?page={page}&pageSize={pageSize}"));
            halResource.Links.Add("create", new HalLink($"/v2") { Method = "POST" });

            if (documentsResults.HasNextPage)
            {
                halResource.Links.Add("next", new HalLink($"/v2?page={page + 1}&pageSize={pageSize}"));
            }

            if (documentsResults.HasPreviousPage)
            {
                halResource.Links.Add("previous", new HalLink($"/v2?page={page - 1}&pageSize={pageSize}"));
            }
        }

        private static string halResourceToJson(HalResource<GetAllDocumentsResponse> halResource)
        {
            var options = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                Converters = { new HalResourceConverter<DocumentSummary>(),  new HalResourceConverter<GetAllDocumentsResponse>() },
                WriteIndented = true
            };
            string json = JsonSerializer.Serialize(halResource, options);
            return json;
        }

        public static HalTemplate GetDocumentTemplate(string title, string method)
        {
            var documentTemplate = new HalTemplate
            {
                Title = title,
                Method = method,
                ContentType = "application/json",
                Properties = new List<HalTemplateProperty>
                {
                    new HalTemplateProperty
                    {
                        Name = "title",
                        Prompt = "Title",
                        Required = true
                    },
                    new HalTemplateProperty
                    {
                        Name = "content",
                        Prompt = "Content",
                        Required = true
                    }
                }
            };

            return documentTemplate;
        }

    }
}