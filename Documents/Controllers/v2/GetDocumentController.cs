using Microsoft.AspNetCore.Mvc;
using Documents.Models;
using Documents.Models.HAL;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authorization;

namespace Documents.Controllers.v2
{
    [Route("/v2")]
    [ApiController]
    [Authorize]
    public class GetDocumentController : ControllerBase
    {
        private readonly IDocumentsQueryBuilder _documentsQueryBuilder;

        public GetDocumentController(IDocumentsQueryBuilder documentsQueryBuilder)
        {
            _documentsQueryBuilder = documentsQueryBuilder;
        }

        [HttpGet("{id}")]
        public IActionResult GetDocument(int id)
        {
            var document = _documentsQueryBuilder.BuildById(id);
            if (document == null)
            {
                return NotFound();
            }

            var halResource = CreateHalResource(document);
            AddLinks(halResource);
            AddTemplates(halResource);

            
            Response.Headers["ETag"] = document.GetHashCode().ToString();
            Response.Headers["Last-Modified"] = document.ModifiedDate.ToString("R");
            Response.Headers["Date"] = document.CreatedDate.ToString("R");

            return Ok(halResourceToJson(halResource));
        }

        private static HalResource<Document> CreateHalResource(Document document)
        {
            return new HalResource<Document>(document);
        }

        private static string halResourceToJson(HalResource<Document> halResource)
        {
            var options = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                Converters = { new HalResourceConverter<Document>() },
                WriteIndented = true
            };
            string json = JsonSerializer.Serialize(halResource, options);
            return json;
        }

        private static void AddTemplates(HalResource<Document> halResource)
        {
            halResource.Templates = new Dictionary<string, HalTemplate>
            {
                { "default", GetDocumentTemplate("Update", "PUT") }
            };	
        }

        private static void AddLinks( HalResource<Document> halResource)
        {
            halResource.Links = new Dictionary<string, HalLink>
            {
                { "self", new HalLink($"/v2/{halResource.Data.Id}") },
                { "update", new HalLink("/v2/{id}") { Templated = true, Method = "PUT" } },
                { "delete", new HalLink("/v2/{id}") { Templated = true, Method = "DELETE" } }
            };

        }

        private static HalTemplate GetDocumentTemplate(string title, string method)
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
                        Name = "id",
                        Prompt = "id",
                        Required = true
                    },
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