using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using ViewComponentsDemo.Mappers;
using ViewComponentsDemo.Models;

namespace ViewComponentsDemo.Services
{
    public interface IWeatherService
    {
        Task<OpenWeatherMapResponse> GetCurrentWeatherAsync(
            string city, string stateAbbrev, TemperatureScale tempScale);
    }

    public class WeatherService : IWeatherService
    {
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _cache;

        public WeatherService(IConfiguration configuration,
                              IMemoryCache cache)
        {
            _configuration = configuration;
            _cache = cache;
        }

        public Task<OpenWeatherMapResponse> GetCurrentWeatherAsync(
            string city, string stateAbbrev, TemperatureScale tempScale)
        {
            const string WEATHER_CACHE_KEY = "Weather";
            string unitsType = (tempScale == TemperatureScale.Celsius) ? "metric" : "imperial";

            // Look for cache key
            if (!_cache.TryGetValue(WEATHER_CACHE_KEY, out OpenWeatherMapResponse currentWeather))
            {
                // Key not in cache, so get data

                // Fetch the user secret
                string apiKey = _configuration.GetValue<string>("OpenWeatherMapApiKey");

                if (String.IsNullOrWhiteSpace(apiKey))
                    throw new ArgumentException("Unable to find an OpenWeatherMap API key in the user secret store.");

                using (var client = new HttpClient())
                {
                    var endpointUrl = $"http://api.openweathermap.org/data/2.5/weather?q={city},{stateAbbrev}&units={unitsType}&appid={apiKey}";
                    var response = client.GetStringAsync(endpointUrl).Result;

                    currentWeather = JsonConvert.DeserializeObject<OpenWeatherMapResponse>(response);
                    //currentWeather = JsonConvert.DeserializeObject<OpenWeatherMapResponse>("{ \"coord\":{ \"lon\":-87.65,\"lat\":41.85},\"weather\":[{\"id\":800,\"main\":\"Clear\",\"description\":\"clear sky\",\"icon\":\"01d\"}],\"base\":\"stations\",\"main\":{\"temp\":288.15,\"pressure\":1030,\"humidity\":87,\"temp_min\":287.15,\"temp_max\":289.15},\"visibility\":16093,\"wind\":{\"speed\":4.6,\"deg\":110},\"clouds\":{\"all\":1},\"dt\":1504962900,\"sys\":{\"type\":1,\"id\":966,\"message\":0.0064,\"country\":\"US\",\"sunrise\":1504956327,\"sunset\":1505002140},\"id\":4887398,\"name\":\"Chicago\",\"cod\":200}");
                }

                // Keep in cache for this duration; reset time if accessed
                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromSeconds(30000000)
                };

                // Save data in cache
                _cache.Set(WEATHER_CACHE_KEY, currentWeather, cacheEntryOptions);
            }

            return Task.Run(() => currentWeather);
        }
    }
}
