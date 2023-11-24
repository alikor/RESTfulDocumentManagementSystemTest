using Xunit;
using System.Text.Json;
using Documents.Models.HAL;

namespace Documents.Tests.Models.HAL
{
    public class TestResource
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class HalResourceConverterTests
    {
        private HalResourceConverter<TestResource> _converter;
        private JsonSerializerOptions _options;

        public HalResourceConverterTests()
        {
            _converter = new HalResourceConverter<TestResource>();
            _options = new JsonSerializerOptions();
        }

        [Fact]
        public void Write_WritesDataProperty()
        {
            // Arrange
            var resource = new HalResource<TestResource>(new TestResource { Id = 1, Name = "Test" });
            var stream = new System.IO.MemoryStream();
            var writer = new Utf8JsonWriter(stream);

            // Act
            _converter.Write(writer, resource, _options);
            writer.Flush();

            // Assert
            var json = System.Text.Encoding.UTF8.GetString(stream.ToArray());
            Assert.Contains("\"Id\":1", json);
            Assert.Contains("\"Name\":\"Test\"", json);
        }

        [Fact]
        public void Write_WritesLinksProperty()
        {
            // Arrange
            var resource = new HalResource<TestResource>(new TestResource { Id = 1, Name = "Test" });
            resource.Links = new Dictionary<string, HalLink> { { "self", new HalLink("/test/1") } };
            var stream = new System.IO.MemoryStream();
            var writer = new Utf8JsonWriter(stream);

            // Act
            _converter.Write(writer, resource, _options);
            writer.Flush();

            // Assert
            var json = System.Text.Encoding.UTF8.GetString(stream.ToArray());
            Assert.Contains("\"_links\":{\"self\":{\"Href\":\"/test/1\"", json);
        }

        [Fact]
        public void Read_ReadsDataProperty()
        {
            // Arrange
            var json = "{\"Id\":1,\"Name\":\"Test\"}";
            var reader = new Utf8JsonReader(System.Text.Encoding.UTF8.GetBytes(json));

            // Act
            var result = _converter.Read(ref reader, typeof(HalResource<TestResource>), _options);

            // Assert
            Assert.Equal(1, result.Data.Id);
            Assert.Equal("Test", result.Data.Name);
        }

        [Fact]
        public void Read_ReadsLinksProperty()
        {
            // Arrange
            var json = "{\"_links\":{\"self\":{\"Href\":\"/test/1\"}}}";
            var reader = new Utf8JsonReader(System.Text.Encoding.UTF8.GetBytes(json));

            // Act
            var result = _converter.Read(ref reader, typeof(HalResource<TestResource>), _options);

            // Assert
            Assert.Equal("/test/1", result.Links["self"].Href);
        }
    }
}
