using Moq;
using Documents.Data.Handlers.Commands;
using Documents.Models;
using Documents.Utitlies;
using Documents.Data.Handlers;

namespace Documents.Tests.Data.Handlers
{
    public class UpdateDocumentCommandHandlerTests
    {
        private readonly Mock<IAppDbContext> _mockContext;
        private readonly Mock<IDateTimeProvider> _mockDateTimeProvider;
        private readonly UpdateDocumentCommandHandler _handler;

        public UpdateDocumentCommandHandlerTests()
        {
            _mockContext = new Mock<IAppDbContext>();
            _mockDateTimeProvider = new Mock<IDateTimeProvider>();
            _handler = new UpdateDocumentCommandHandler(_mockContext.Object, _mockDateTimeProvider.Object);
        }

        [Fact]
        public async Task Handle_ShouldUpdateDocument()
        {
            // Arrange
            var command = new UpdateDocumentCommand(1,"New Title",  "New Content" );
            var document = new Document { Id = 1, Title = "Old Title", Content = "Old Content" };

            _mockContext.Setup(x => x.Documents.FindAsync(command.Id)).ReturnsAsync(document);
            _mockDateTimeProvider.Setup(x => x.UtcNow).Returns(DateTime.UtcNow);

            // Act
            await _handler.Handle(command);

            // Assert
            Assert.Equal(command.Title, document.Title);
            Assert.Equal(command.Content, document.Content);
            _mockContext.Verify(d => d.SaveChangesAsync(default), Times.Once);
        }
    }
}