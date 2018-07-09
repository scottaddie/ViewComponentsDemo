using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ViewComponentsDemo.Mappers;
using ViewComponentsDemo.Models;
using ViewComponentsDemo.Services;
using static ViewComponentsDemo.Mappers.WeatherMapper;
using VM = ViewComponentsDemo.ViewModels;

namespace ViewComponentsDemo.ViewComponents
{
    public class CurrentWeather : ViewComponent
    {
        private readonly IWeatherService _service;

        public CurrentWeather(IWeatherService service) => 
            _service = service;

        public async Task<IViewComponentResult> InvokeAsync(
            string city, string countryCode, 
            TemperatureScale tempScale, Language lang)
        {
            OpenWeatherMapResponse currentWeather =
                await _service.GetCurrentWeatherAsync(
                    city?.Trim(), countryCode?.Trim(), tempScale, lang);
            VM.Weather weather = currentWeather.MapToWeather(tempScale);

            return View(weather);
        }
    }
}