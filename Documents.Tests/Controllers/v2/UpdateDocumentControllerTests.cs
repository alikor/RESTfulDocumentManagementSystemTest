using Moq;
using Documents.Data.Handlers.Commands;
using Documents.Data.Handlers;
using Microsoft.AspNetCore.Mvc;

namespace Documents.Tests.Controllers.v2
{

    public class UpdateDocumentControllerTests
    {
        private readonly Mock<IUpdateDocumentCommandHandler> _mockCommandHandler;
        private readonly UpdateDocumentController _controller;

        public UpdateDocumentControllerTests()
        {
            _mockCommandHandler = new Mock<IUpdateDocumentCommandHandler>();
            _controller = new UpdateDocumentController(_mockCommandHandler.Object);
        }

        [Fact]
        public async Task UpdateDocument_ReturnsNoContentResult()
        {
            // Arrange
            var command = new UpdateDocumentCommand(123, "title", "content");

            // Act
            var result = await _controller.UpdateDocument(123, command);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}