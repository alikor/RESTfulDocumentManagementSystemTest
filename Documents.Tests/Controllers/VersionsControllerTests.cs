using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Documents.Controllers.v2;
using Documents.Models.HAL;
using System.Text.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Documents.Tests.Controllers
{
    public class VersionsControllerTests
    {

        [Fact]
        public void GetVersions_ReturnsOkResult()
        {
            // Arrange
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "username")
            }, "TestAuthentication"));

            var httpContext = new DefaultHttpContext()
            {
                User = user
            };

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            var controller = new VersionsController
            {
                ControllerContext = controllerContext
            };

            // Act
            var result = controller.GetVersions();

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetVersions_ReturnsCorrectJson()
        {
            // Arrange
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "username"),
            }, "TestAuthentication"));

            var httpContext = new DefaultHttpContext()
            {
                User = user
            };

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            var controller = new VersionsController
            {
                ControllerContext = controllerContext
            };

            // Act
            var result = controller.GetVersions() as OkObjectResult;

            // Assert
            var options = new JsonSerializerOptions
            {
                Converters = { new HalResourceConverter<object>() }
            };
            var halResource = JsonSerializer.Deserialize<HalResource<object>>(result.Value.ToString(), options);
            Assert.NotNull(halResource);
            Assert.True(halResource.Links.ContainsKey("self"), "self link is missing");
            Assert.True(halResource.Links.ContainsKey("v1"), "v1 link is missing");
            Assert.True(halResource.Links.ContainsKey("v2"), "v2 link is missing");
            Assert.True(halResource.Links.ContainsKey("latest"), "latest link is missing");
            Assert.False(halResource.Links.ContainsKey("token"), "token link is present");
        }

        [Fact]
        public void GetVersions_unauthenticatedUser_ReturnsCorrectJson() {
            // Arrange
            var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());

            var httpContext = new DefaultHttpContext()
            {
                User = anonymousUser
            };

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            var controller = new VersionsController
            {
                ControllerContext = controllerContext
            };

            // Act
            var result = controller.GetVersions() as OkObjectResult;

            // Assert
            var options = new JsonSerializerOptions
            {
                Converters = { new HalResourceConverter<object>() }
            };
            var halResource = JsonSerializer.Deserialize<HalResource<object>>(result.Value.ToString(), options);
            Assert.NotNull(halResource);
            Assert.True(halResource.Links.ContainsKey("self"), "self link is missing");
            Assert.False(halResource.Links.ContainsKey("v1"), "v1 link is present");
            Assert.False(halResource.Links.ContainsKey("v2"), "v2 link is present");
            Assert.False(halResource.Links.ContainsKey("latest"), "latest link is present");
            Assert.True(halResource.Links.ContainsKey("authenticate"), "authenticate link is missing");
        }
    }
}