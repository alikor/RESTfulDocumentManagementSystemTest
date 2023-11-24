using Xunit;
using Moq;
using Documents.Data.Handlers;
using Documents.Data.Handlers.Commands;
using Documents.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Documents.Data.Handlers.Commands;

namespace Documents.Tests.Controllers.v2
{

    public class CreateDocumentControllerTests
    {
        [Fact]
        public async Task CreateDocument_ReturnsCreatedAtActionResult()
        {
            // Arrange 
            var mockCommandHandler = new Mock<ICreateDocumentCommandHandler>();
            var controller = new CreateDocumentController(mockCommandHandler.Object);
            var command = new CreateDocumentCommand("title", "content");

            mockCommandHandler.Setup(x => x.Handle(command)).ReturnsAsync(123);


            // Act
            var result = await controller.CreateDocument(command);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("CreateDocument", createdAtActionResult.ActionName);
            Assert.Equal(123, createdAtActionResult.RouteValues["id"]);

        }
    }
}