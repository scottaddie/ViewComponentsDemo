using Microsoft.AspNetCore.Mvc;
using ViewComponentsDemo.Mappers;
using ViewComponentsDemo.ViewComponents;

namespace ViewComponentsDemo.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();

        public IActionResult TagHelperInvocation() => View();

        public IActionResult ControllerInvocation() => 
            ViewComponent(nameof(CurrentWeather), new
            {
                city = "Chicago",
                stateAbbrev = "IL",
                tempScale = TemperatureScale.Celsius
            });

        public IActionResult Error() => View();
    }
}
