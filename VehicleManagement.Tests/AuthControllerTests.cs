using Xunit;
using Moq;
using VehicleManagement.Api.Controllers;
using VehicleManagement.Api.Services.Interfaces;
using VehicleManagement.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace VehicleManagement.Tests
{
    public class AuthControllerTests
    {
        [Fact]
        public async Task Login_InvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            var mockAuthService = new Mock<IAuthService>();
            mockAuthService.Setup(s => s.AuthenticateAsync("user", "wrong")).ReturnsAsync((User)null);
            var mockLogger = new Mock<Microsoft.Extensions.Logging.ILogger<AuthController>>();
            var controller = new AuthController(mockAuthService.Object, mockLogger.Object);
            var request = new LoginRequest { Username = "user", Password = "wrong" };

            // Act
            var result = await controller.Login(request);

            // Assert
            var unauthorized = Assert.IsType<UnauthorizedObjectResult>(result);
            var error = Assert.IsType<ErrorResponse>(unauthorized.Value);
            Assert.Contains("Invalid credentials", error.Errors);
        }
    }
}
