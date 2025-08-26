using Xunit;
using Moq;
using VehicleManagement.Api.Controllers;
using VehicleManagement.Api.Models;
using VehicleManagement.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VehicleManagement.Tests
{
    public class VehiclesControllerTests
    {
        [Fact]
        public async Task Get_ReturnsVehicles()
        {
            var mockService = new Mock<IVehicleService>();
            mockService.Setup(s => s.GetVehiclesAsync(null, null)).ReturnsAsync(new List<Vehicle> { new Vehicle { Id = "1", Model = "ModelX", Plate = "ABC1234", Year = 2023 } });
            var mockLogger = new Mock<Microsoft.Extensions.Logging.ILogger<VehiclesController>>();
            var controller = new VehiclesController(mockService.Object, mockLogger.Object);

            var result = await controller.Get(null, null);

            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var vehicles = Assert.IsAssignableFrom<IEnumerable<Vehicle>>(ok.Value);
            Assert.Single(vehicles);
        }

        [Fact]
        public async Task Post_InvalidModel_ReturnsBadRequest()
        {
            var mockService = new Mock<IVehicleService>();
            var mockLogger = new Mock<Microsoft.Extensions.Logging.ILogger<VehiclesController>>();
            var controller = new VehiclesController(mockService.Object, mockLogger.Object);
            var vehicle = new Vehicle { Model = "", Plate = "ABC1234", Year = 2023 };

            var result = await controller.Post(vehicle);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var error = Assert.IsType<ErrorResponse>(badRequest.Value);
            Assert.Contains("Model is required.", error.Errors);
        }
    }
}
