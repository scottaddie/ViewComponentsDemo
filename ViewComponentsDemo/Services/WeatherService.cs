using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using ViewComponentsDemo.Models;

namespace ViewComponentsDemo.Services
{
    public interface IWeatherService
    {
        Task<OpenWeatherMapResponse> GetCurrentWeatherAsync(string city, string stateAbbrev);
    }

    public class WeatherService : IWeatherService
    {
        private readonly IConfiguration _configuration;

        public WeatherService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task<OpenWeatherMapResponse> GetCurrentWeatherAsync(string city, string stateAbbrev)
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
