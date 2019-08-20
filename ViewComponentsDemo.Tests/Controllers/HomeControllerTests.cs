using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ViewComponentsDemo.Controllers;
using Xunit;

namespace ViewComponentsDemo.Tests.Controllers
{
    public class HomeControllerTests
    {
        private readonly IConfigurationRoot _configuration;

        public HomeControllerTests()
        {
            _configuration = TestHelper.InitConfiguration();
        }

        [Fact]
        public void ControllerInvocation_Returns_ViewComponentResult()
        {
            // Arrange
            var controller = new HomeController();

            // Act
            var result = controller.ControllerInvocation(_configuration);

            // Assert
            Assert.IsType<ViewComponentResult>(result);
        }
    }
}
