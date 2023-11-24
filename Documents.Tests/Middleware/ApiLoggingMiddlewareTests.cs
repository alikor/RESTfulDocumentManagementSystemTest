using Moq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Documents.Middleware;

namespace Documents.Tests.Middleware
{

    public class ApiLoggingMiddlewareTests
    {

        [Fact]
        public async Task Invoke_CallsNextDelegate()
        {
            var nextMock = new Mock<RequestDelegate>();
            var loggerMock = new Mock<ILogger<ApiLoggingMiddleware>>();
            var middleware = new ApiLoggingMiddleware(nextMock.Object, loggerMock.Object);
            var context = new DefaultHttpContext();

            await middleware.Invoke(context);

            nextMock.Verify(next => next(context), Times.Once);
        }

        [Fact]
        public async Task Invoke_LogsElapsedTime()
        {
            var nextMock = new Mock<RequestDelegate>();
            var loggerMock = new Mock<ILogger<ApiLoggingMiddleware>>();
            var middleware = new ApiLoggingMiddleware(nextMock.Object, loggerMock.Object);
            var context = new DefaultHttpContext();

            await middleware.Invoke(context);

            loggerMock.Verify(
                x => x.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                Times.Once);

        }
    }
}