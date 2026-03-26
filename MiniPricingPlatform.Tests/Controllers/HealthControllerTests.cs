using Microsoft.AspNetCore.Mvc;
using MiniPricingPlatform.API.Controllers;
using MiniPricingPlatform.API.DTOs;

namespace MiniPricingPlatform.API.Tests.Controllers
{
    public class HealthControllerTests
    {
        private readonly HealthController _controller;

        public HealthControllerTests()
        {
            _controller = new HealthController();
        }

        [Fact]
        public void CallHealthCheck_ShouldReturn_StatusOk()
        {
            // Act
            var result = _controller.Check();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var data = Assert.IsType<HealthResponseDto>(okResult.Value);
            Assert.Equal("ok", data.Status);
        }


    }
}