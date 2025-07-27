using Microsoft.Extensions.Logging;
using Moq;

namespace SmartAccess.Tests.Helpers
{
    public static class LoggerExtensions
    {
        public static void VerifyLog<T>(
            this Mock<ILogger<T>> logger,
            LogLevel level,
            string containsMessage)
        {
            logger.Verify(
                x => x.Log(
                    level,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains(containsMessage)),
                    It.IsAny<Exception?>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }
    }
}
