using Moq;
using Microsoft.AspNetCore.Mvc;
using Documents.Models;
using Documents.Controllers.v2;
using Documents.Data.Quries;
using Documents.Controllers.v2.Dtos;
using Documents.Models.HAL;
using System.Text.Json;

namespace Documents.Tests.Controllers.v2
{

    public class GetDocumentsControllerTests
    {
        private readonly Mock<IDocumentsQueryBuilder> _mockDocumentsQueryBuilder;
        private readonly GetDocumentsController _controller;

        private JsonSerializerOptions _options;

        public GetDocumentsControllerTests()
        {
            _mockDocumentsQueryBuilder = new Mock<IDocumentsQueryBuilder>();
            _controller = new GetDocumentsController(_mockDocumentsQueryBuilder.Object);

            _options = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                Converters = { new HalResourceConverter<DocumentSummary>(),  new HalResourceConverter<GetAllDocumentsResponse>() },
                WriteIndented = true
            };
        }

        [Fact]
        public void GetAllDocuments_ReturnsNextPageLink_WhenHasNextPageIsTrue()
        {
            // Arrange
            var page = 1;
            var pageSize = 10;
            var totalCount = 100;
            var pagedResult = new PagedResult<Document>(totalCount, page, pageSize);
            _mockDocumentsQueryBuilder.Setup(d => d.Build(page, pageSize)).Returns(pagedResult);

            // Act
            var result = _controller.GetAllDocuments(page, pageSize) as OkObjectResult;

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var halResource = JsonSerializer.Deserialize<HalResource<GetAllDocumentsResponse>>(result.Value.ToString(), _options);
            Assert.NotNull(halResource);
            Assert.True(halResource.Links.ContainsKey("next"));
        }

        [Fact]
        public void GetAllDocuments_ReturnsNextPageLink_WhenHasNextPageIsFalse()
        {
            // Arrange
            var page = 1;
            var pageSize = 10;
            var totalCount = 1;
            var pagedResult = new PagedResult<Document>(totalCount, page, pageSize);
            _mockDocumentsQueryBuilder.Setup(d => d.Build(page, pageSize)).Returns(pagedResult);

            // Act
            var result = _controller.GetAllDocuments(page, pageSize) as OkObjectResult;

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var halResource = JsonSerializer.Deserialize<HalResource<GetAllDocumentsResponse>>(result.Value.ToString(), _options);
            Assert.NotNull(halResource);
            Assert.False(halResource.Links.ContainsKey("next"));
        }

        [Fact]
        public void GetAllDocuments_ReturnsPreviousPageLink_WhenHasPreviousPageIsTrue()
        {
            // Arrange
            var page = 2;
            var pageSize = 10;
            var totalCount = 100;
            var pagedResult = new PagedResult<Document>(totalCount, page, pageSize);
            _mockDocumentsQueryBuilder.Setup(d => d.Build(page, pageSize)).Returns(pagedResult);

            // Act
            var result = _controller.GetAllDocuments(page, pageSize) as OkObjectResult;

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var halResource = JsonSerializer.Deserialize<HalResource<GetAllDocumentsResponse>>(result.Value.ToString(), _options);
            Assert.NotNull(halResource);
            Assert.True(halResource.Links.ContainsKey("previous"));
        }

        [Fact]
        public void GetAllDocuments_ReturnsOk_WhenDocumentsExist()
        {
            // Arrange
            var page = 1;
            var pageSize = 10;
            var totalCount = 100;
            var documents = new List<Document>
            {
                new Document { Id = 1, Title = "Document 1" },
                new Document { Id = 2, Title = "Document 2" }
            };
            var pagedResult = new PagedResult<Document>(totalCount, page, pageSize) { Documents = documents };
            _mockDocumentsQueryBuilder.Setup(d => d.Build(page, pageSize)).Returns(pagedResult);

            // Act
            var result = _controller.GetAllDocuments(page, pageSize) as OkObjectResult;

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var json = result.Value.ToString();
            var halResource = JsonSerializer.Deserialize<HalResource<GetAllDocumentsResponse>>(json, _options);
            Assert.NotNull(halResource);
            Assert.Equal(totalCount, halResource.Data.TotalCount);
            Assert.Equal(page, halResource.Data.PageNumber);
            Assert.Equal(pageSize, halResource.Data.PageSize);

            // assert docuements have been embedded
            Assert.NotNull(halResource.Embedded);
            Assert.True(halResource.Embedded.ContainsKey("documents"));
            var embeddedDocuments = JsonSerializer.Deserialize<IEnumerable<HalResource<DocumentSummary>>>(halResource.Embedded["documents"].ToString(), _options);
            Assert.NotNull(embeddedDocuments);
            Assert.Equal(documents.Count, embeddedDocuments.Count());

            // assert that embeded documents have self links
            foreach (var embeddedDocument in embeddedDocuments)
            {
                Assert.True(embeddedDocument.Links.ContainsKey("self"));
            }

            // assert links have been added
            Assert.True(halResource.Links.ContainsKey("self"));
            Assert.True(halResource.Links.ContainsKey("create"));
            Assert.True(halResource.Links.ContainsKey("next"));

        }

    }
}