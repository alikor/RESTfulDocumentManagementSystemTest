using Moq;
using Documents.Data.Quries;
using Documents.Models;

namespace Documents.Tests.Data.Quries
{
    public class DocumentsQueryBuilderTests
    {
        private readonly Mock<IAppDbContext> _mockContext;
        private readonly DocumentsQueryBuilder _queryBuilder;

        public DocumentsQueryBuilderTests()
        {
            _mockContext = new Mock<IAppDbContext>();
            _queryBuilder = new DocumentsQueryBuilder(_mockContext.Object);
        }

        [Fact]
        public void Build_ReturnsPagedResult()
        {
            // Arrange
            var documents = new List<Document>
            {
                new Document { Id = 1 },
                new Document { Id = 2 },
                new Document { Id = 3 },
                new Document { Id = 4 },
                new Document { Id = 5 }
            }.AsQueryable();

            var mockSet = new Mock<Microsoft.EntityFrameworkCore.DbSet<Document>>();
            mockSet.As<IQueryable<Document>>().Setup(m => m.Provider).Returns(documents.Provider);
            mockSet.As<IQueryable<Document>>().Setup(m => m.Expression).Returns(documents.Expression);
            mockSet.As<IQueryable<Document>>().Setup(m => m.ElementType).Returns(documents.ElementType);
            mockSet.As<IQueryable<Document>>().Setup(m => m.GetEnumerator()).Returns(documents.GetEnumerator());


            _mockContext.Setup(db => db.Documents).Returns( mockSet.Object);

            // Act
            var result = _queryBuilder.Build(1, 3);

            // Assert
            Assert.Equal(3, result.Documents.Count);
            Assert.Equal(5, result.TotalCount);
            Assert.Equal(1, result.PageNumber);
            Assert.Equal(3, result.PageSize);
        }

        [Fact]
        public void BuildById_ReturnsDocument()
        {
            // Arrange
            var documents = new List<Document>
            {
                new Document { Id = 1 },
                new Document { Id = 2 },
                new Document { Id = 3 }
            }.AsQueryable();

            var mockSet = new Mock<Microsoft.EntityFrameworkCore.DbSet<Document>>();
            mockSet.As<IQueryable<Document>>().Setup(m => m.Provider).Returns(documents.Provider);
            mockSet.As<IQueryable<Document>>().Setup(m => m.Expression).Returns(documents.Expression);
            mockSet.As<IQueryable<Document>>().Setup(m => m.ElementType).Returns(documents.ElementType);
            mockSet.As<IQueryable<Document>>().Setup(m => m.GetEnumerator()).Returns(documents.GetEnumerator());


            _mockContext.Setup(db => db.Documents).Returns(mockSet.Object);

            // Act
            var result = _queryBuilder.BuildById(2);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Id);
        }
    }
}