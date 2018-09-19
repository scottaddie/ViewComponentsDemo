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
        private readonly WeatherService _service;

        public CurrentWeather(WeatherService service) => 
            _service = service;

        public async Task<IViewComponentResult> InvokeAsync(
            string city, string countryCode, 
            TemperatureScale tempScale, Language lang)
        {
            var request = new ForecastRequest
            {
                City = city?.Trim(),
                CountryCode = countryCode.Trim(),
                TemperatureScale = tempScale.ToUnitsType(),
                LanguageCode = lang.ToLanguageCode()
            };

            Forecast currentWeather =
                await _service.GetCurrentWeatherAsync(request);
            VM.Weather weather = currentWeather?.MapToWeather(tempScale);

            return View(weather);
        }
    }
}
