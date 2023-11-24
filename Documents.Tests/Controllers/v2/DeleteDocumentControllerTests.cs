using Moq;
using Documents.Data.Handlers;
using Documents.Data.Handlers.Commands;
using Microsoft.AspNetCore.Mvc;


namespace Documents.Tests.Controllers.v2
{
    public class DeleteDocumentControllerTests
    {
        private readonly DeleteDocumentController _controller;
        private readonly Mock<IDeleteDocumentCommandHandler> _handlerMock;

        public DeleteDocumentControllerTests()
        {
            _handlerMock = new Mock<IDeleteDocumentCommandHandler>();
            _controller = new DeleteDocumentController(_handlerMock.Object);
        }

        [Fact]
        public async Task DeleteDocument_ReturnsNoContent()
        {
            // Arrange
            int testId = 1;
            _handlerMock.Setup(h => h.Handle(It.IsAny<DeleteDocumentCommand>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteDocument(testId);

            // Assert
            _handlerMock.Verify(h => h.Handle(It.Is<DeleteDocumentCommand>(c => c.Id == testId)), Times.Once);
            Assert.IsType<NoContentResult>(result);
        }
    }
}