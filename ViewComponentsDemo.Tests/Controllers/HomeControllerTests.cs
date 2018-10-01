using Microsoft.AspNetCore.Mvc;
using ViewComponentsDemo.Controllers;
using Xunit;

namespace ViewComponentsDemo.Tests.Controllers
{
    public class HomeControllerTests
    {
        [Fact]
        public void ControllerInvocation_Returns_ViewComponentResult()
        {
            // Arrange
            var controller = new HomeController();

            // Act
            var result = controller.ControllerInvocation();

            // Assert
            Assert.IsType<ViewComponentResult>(result);
        }
    }
}
