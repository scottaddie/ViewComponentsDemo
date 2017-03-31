using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using ViewComponentsDemo.Mappers;
using ViewComponentsDemo.Models;
using VM = ViewComponentsDemo.ViewModels;

namespace ViewComponentsDemo.ViewComponents
{
    public class CurrentWeatherViewComponent : ViewComponent
    {
        private readonly IConfiguration _configuration;

        public CurrentWeatherViewComponent(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IViewComponentResult> InvokeAsync(string city, string stateAbbrev)
        {
            OpenWeatherMapResponse currentWeather = await GetWeatherAsync(city?.Trim(), stateAbbrev?.Trim());
            VM.Weather weather = currentWeather.MapToWeather();

            return View(weather);
        }

        private Task<OpenWeatherMapResponse> GetWeatherAsync(string city, string stateAbbrev)
        {
            // Fetch the user secret
            string apiKey = _configuration.GetValue<string>("OpenWeatherMapApiKey");

            OpenWeatherMapResponse currentWeather = null;

            using (var client = new HttpClient())
            {
                var endpointUrl = $"http://api.openweathermap.org/data/2.5/weather?q={city},{stateAbbrev}&appid={apiKey}";
                var response = client.GetStringAsync(endpointUrl).Result;
                
                currentWeather = JsonConvert.DeserializeObject<OpenWeatherMapResponse>(response);
            }

            return Task.Run(() => { return currentWeather; });
        }
    }
}
