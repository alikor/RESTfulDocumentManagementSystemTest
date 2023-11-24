using Moq;

using Documents.Data.Handlers.Commands;
using Documents.Data.Handlers;
using Documents.Models;

namespace Documents.Tests.Data.Handlers
{
    public class DeleteDocumentCommandHandlerTests
    {
        [Fact]
        public async Task Handle_CallsAttachAndRemoveAndSaveChangesAsync()
        {
            // Arrange
            var dbContextMock = new Mock<IAppDbContext>();
            var documentsMock = new Mock<Microsoft.EntityFrameworkCore.DbSet<Document>>();
            dbContextMock.Setup(d => d.Documents ).Returns(documentsMock.Object);
            var command = new DeleteDocumentCommand(1);
            var handler = new DeleteDocumentCommandHandler(dbContextMock.Object);

            // Act
            await handler.Handle(command);

            // Assert
            documentsMock.Verify(s => s.Attach(It.Is<Document>(d => d.Id == command.Id)), Times.Once);
            documentsMock.Verify(s => s.Remove(It.Is<Document>(d => d.Id == command.Id)), Times.Once);
            dbContextMock.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        }
    }
}