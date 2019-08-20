using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ViewComponentsDemo.Mappers;
using ViewComponentsDemo.ViewComponents;

namespace ViewComponentsDemo.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();

        public IActionResult TagHelperInvocation() => View();

        public IActionResult ControllerInvocation([FromServices] IConfiguration config) => 
            ViewComponent(nameof(CurrentWeather), new
            {
                city = config["Weather:City"],
                countryCode = config["Weather:CountryCode"],
                tempScale = TemperatureScale.Fahrenheit,
                lang = Language.French,
            });

        public IActionResult Error() => View();
    }
}
