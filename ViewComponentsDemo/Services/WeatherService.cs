using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
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
        private readonly IMemoryCache _cache;

        public WeatherService(IConfiguration configuration,
                              IMemoryCache cache)
        {
            _configuration = configuration;
            _cache = cache;
        }

        public Task<OpenWeatherMapResponse> GetCurrentWeatherAsync(string city, string stateAbbrev)
        {
            const string WEATHER_CACHE_KEY = "Weather";

            // Look for cache key
            if (!_cache.TryGetValue(WEATHER_CACHE_KEY, out OpenWeatherMapResponse currentWeather))
            {
                // Key not in cache, so get data

                // Fetch the user secret
                string apiKey = _configuration.GetValue<string>("OpenWeatherMapApiKey");

                using (var client = new HttpClient())
                {
                    var endpointUrl = $"http://api.openweathermap.org/data/2.5/weather?q={city},{stateAbbrev}&appid={apiKey}";
                    var response = client.GetStringAsync(endpointUrl).Result;

                    currentWeather = JsonConvert.DeserializeObject<OpenWeatherMapResponse>(response);
                }

                // Keep in cache for this duration; reset time if accessed
                var cacheEntryOptions = new MemoryCacheEntryOptions {
                    SlidingExpiration = TimeSpan.FromSeconds(10)
                };

                // Save data in cache
                _cache.Set(WEATHER_CACHE_KEY, currentWeather, cacheEntryOptions);
            }

            return Task.Run(() => { return currentWeather; });
        }
    }
}
