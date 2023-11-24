using Microsoft.AspNetCore.Mvc;
using Documents.Models.HAL;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;

namespace Documents.Controllers.v2
{
    [Route("/")]
    [ApiController]
    public class VersionsController : ControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetVersions()
        {
            var halResource = new HalResource<object>(new object()
            );

            halResource.Links.Add("self", new HalLink { Href = "/" });
            
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                halResource.Links.Add("v1", new HalLink { Href = "/v1", Deprecation = "/v2" });
                halResource.Links.Add("v2", new HalLink { Href = "/v2?page={page}&pageSize={pageSize}", Templated = true });
                halResource.Links.Add("latest", new HalLink { Href = "/v2?page={page}&pageSize={pageSize}", Templated = true });
            }
            else
            {
                
                var documentTemplate = new HalTemplate
                {
                    Title = "authenticate",
                    Method = "POST",
                    ContentType = "application/json",
                    Properties = new List<HalTemplateProperty>
                        {
                            new HalTemplateProperty
                            {
                                Name = "username",
                                Prompt = "username (default admin)",
                                Required = true
                            },
                            new HalTemplateProperty
                            {
                                Name = "emailAddress",
                                Prompt = "emailAddress",
                                Required = true
                            }
                        }
                };

                halResource.Templates = new Dictionary<string, HalTemplate>() { { "default", documentTemplate } };	
                halResource.Links.Add("authenticate", new HalLink { Href = "/token", Method = "POST" });
            }
            


            var options = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                Converters = { new HalResourceConverter<object>() },
                WriteIndented = true
                
            };
            string json = JsonSerializer.Serialize(halResource, options);

            return Ok(json);

        }
    }


}