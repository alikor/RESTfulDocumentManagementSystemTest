using Moq;
using Documents.Controllers.v2;
using Documents.Models;
using Microsoft.AspNetCore.Mvc;
using Documents.Models.HAL;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace Documents.Tests.Controllers.v2
{

    public class GetDocumentControllerTests
    {
        private JsonSerializerOptions _options;

        public GetDocumentControllerTests()
        {
            this._options = new JsonSerializerOptions
            {
                Converters = { new HalResourceConverter<Document>() }
            };
        }

        [Fact]
        public void GetDocument_ReturnsNotFound_WhenDocumentDoesNotExist()
        {
            // Arrange
            var mockQueryBuilder = new Mock<IDocumentsQueryBuilder>();
            mockQueryBuilder.Setup(qb => qb.BuildById(It.IsAny<int>())).Returns((Document)null);
            var controller = new GetDocumentController(mockQueryBuilder.Object);

            // Act
            var result = controller.GetDocument(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void GetDocument_ReturnsOk_WhenDocumentExists()
        {
            // Arrange
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(m => m.Response.Headers).Returns(new HeaderDictionary());
            var controllerContext = new ControllerContext()
            {
                HttpContext = mockHttpContext.Object,
            };
            var mockQueryBuilder = new Mock<IDocumentsQueryBuilder>();
            var document = new Document
            {
                Id = 1,
                Title = "Test Document",
                Content = "This is a test document."
            };
            mockQueryBuilder.Setup(qb => qb.BuildById(It.IsAny<int>())).Returns(document);
            var controller = new GetDocumentController(mockQueryBuilder.Object)
            {
                ControllerContext = controllerContext,
            };

            // Act
            var result = controller.GetDocument(1) as OkObjectResult;

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(result);
            var halResource = JsonSerializer.Deserialize<HalResource<Document>>(result.Value.ToString(), _options);
            Assert.NotNull(halResource);
            var returnedDocument = (Document)halResource.Data;
            Assert.Equal(document.Id, returnedDocument.Id);
            Assert.Equal(document.Title, returnedDocument.Title);
            Assert.Equal(document.Content, returnedDocument.Content);

            Assert.Equal(document.GetHashCode().ToString(), controller.Response.Headers["ETag"]);
            Assert.Equal(document.ModifiedDate.ToString("R"), controller.Response.Headers["Last-Modified"]);
            Assert.Equal(document.CreatedDate.ToString("R"), controller.Response.Headers["Date"]);

            // assert self link
            Assert.True(halResource.Links.ContainsKey("self"));
            Assert.Equal($"/v2/{document.Id}", halResource.Links["self"].Href);
            
        }
    }
}