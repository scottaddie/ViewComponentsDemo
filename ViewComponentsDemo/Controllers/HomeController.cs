using Microsoft.AspNetCore.Mvc;

namespace ViewComponentsDemo.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();

        public IActionResult About() => View();

        public IActionResult Contact()
        {
            return ViewComponent("CurrentWeather", new
            {
                city = "Chicago",
                stateAbbrev = "IL"
            });
        }

        public IActionResult Error() => View();
    }
}
