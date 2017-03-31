using Microsoft.AspNetCore.Mvc;
using ViewComponentsDemo.ViewComponents;

namespace ViewComponentsDemo.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();

        public IActionResult About() => View();

        public IActionResult Contact()
        {
            return ViewComponent(nameof(CurrentWeather), new
            {
                city = "Chicago",
                stateAbbrev = "IL"
            });
        }

        public IActionResult Error() => View();
    }
}
