

using Documents.Controllers;
using Documents.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Documents.Tests.Controllers
{
    public class TokenControllerTests
    {
        private readonly TokenController _controller;
        private readonly Mock<IConfiguration> _config;
        private readonly LoginModel _loginModel;

        public TokenControllerTests()
        {
            _config = new Mock<IConfiguration>();
            _config.Setup(c => c["Jwt:Key"]).Returns("//Yarg+EjAwWP3MELCr8lJUdEaI8Dod9tVA52S+zt4U=");
            _config.Setup(c => c["Jwt:Issuer"]).Returns("test_issuer");

            _controller = new TokenController(_config.Object);
            
        }

        [Fact]
        public void CreateToken_ReturnsUnauthorizedResult_WhenUserIsNull()
        {
            var loginModel = new LoginModel { Username = "", EmailAddress = ""};

            var result = _controller.CreateToken(loginModel);

            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public void CreateToken_ReturnsOkResult_WhenUserIsNotNull()
        {
            var loginModel = new LoginModel { Username = "Admin", EmailAddress = ""};


            var result = _controller.CreateToken(loginModel);

            Assert.IsType<OkObjectResult>(result);
        }
    }
}