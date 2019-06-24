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
                city = "Milwaukee",
                countryCode = "US",
                tempScale = TemperatureScale.Fahrenheit,
                lang = Language.French
            });

        public IActionResult Error() => View();
    }
}
