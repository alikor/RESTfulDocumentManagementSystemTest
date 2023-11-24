using Moq;
using Xunit;
using Documents.Data.Handlers.Commands;
using Documents.Models;
using System;
using System.Threading.Tasks;
using Documents.Data.Handlers;
using System.Collections.Generic;
using Documents.Utitlies;

namespace Documents.Tests.Data.Handlers
{
    public class CreateDocumentCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldCreateDocument()
        {
            // Arrange
            var dbContextMock = new Mock<IAppDbContext>();
            var documentsMock = new Mock<Microsoft.EntityFrameworkCore.DbSet<Document>>();
            dbContextMock.Setup(d => d.Documents ).Returns(documentsMock.Object);
            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            var now = DateTime.UtcNow;
            dateTimeProviderMock.Setup(d => d.UtcNow).Returns(now);
            var handler = new CreateDocumentCommandHandler(dbContextMock.Object, dateTimeProviderMock.Object);
            var command = new CreateDocumentCommand("Test Title", "Test Content" );

            // Act
            await handler.Handle(command);

            // Assert
            documentsMock.Verify(d => d.Add(It.Is<Document>(doc => doc.Title == command.Title && doc.Content == command.Content && doc.CreatedDate == now)), Times.Once);
            dbContextMock.Verify(d => d.SaveChangesAsync(default), Times.Once);
        }
    }
}