using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ViewComponentsDemo.Mappers;
using ViewComponentsDemo.Models;
using ViewComponentsDemo.Services;
using VM = ViewComponentsDemo.ViewModels;

namespace ViewComponentsDemo.ViewComponents
{
    public class CurrentWeatherViewComponent : ViewComponent
    {
        private readonly IWeatherService _service;

        public CurrentWeatherViewComponent(IWeatherService service)
        {
            _service = service;
        }

        public async Task<IViewComponentResult> InvokeAsync(string city, string stateAbbrev)
        {
            OpenWeatherMapResponse currentWeather = await _service.GetCurrentWeatherAsync(city?.Trim(), stateAbbrev?.Trim());
            VM.Weather weather = currentWeather.MapToWeather();

            return View(weather);
        }
    }
}