using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ViewComponentsDemo.Mappers;
using ViewComponentsDemo.Models;

namespace ViewComponentsDemo.Services
{
    public interface IWeatherService
    {
        Task<OpenWeatherMapResponse> GetCurrentWeatherAsync(
            string city, string countryCode, TemperatureScale tempScale, Language lang);
    }

    public class WeatherService : IWeatherService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClient;
        private readonly IMemoryCache _cache;

        public WeatherService(IConfiguration configuration,
                              IHttpClientFactory httpClient,
                              IMemoryCache cache)
        {
            _configuration = configuration;
            _httpClient = httpClient;
            _cache = cache;
        }

        public async Task<OpenWeatherMapResponse> GetCurrentWeatherAsync(
            string city, string countryCode, TemperatureScale tempScale, Language lang)
        {
            const string WEATHER_CACHE_KEY = "Weather";

            // Look for cache key
            if (!_cache.TryGetValue(WEATHER_CACHE_KEY, out OpenWeatherMapResponse currentWeather))
            {
                // Key not in cache, so get data

                // Fetch the user secret
                var apiKey = _configuration.GetValue<string>("OpenWeatherMapApiKey");

                if (String.IsNullOrWhiteSpace(apiKey))
                {
                    throw new ArgumentException("Unable to find an OpenWeatherMap API key in the user secret store.");
                }

                string langCode = lang.ToLanguageCode();
                string unitsType = tempScale.ToUnitsType();
                IConfigurationSection weatherConfig = _configuration.GetSection("Weather");
                var baseUrl = weatherConfig.GetValue<string>("ApiBaseUrl");
                var endpointUrl = $"{baseUrl}?q={city},{countryCode}&lang={langCode}&units={unitsType}&appid={apiKey}";

                var client = _httpClient.CreateClient();
                var response = await client.GetStringAsync(endpointUrl);

                currentWeather = JsonConvert.DeserializeObject<OpenWeatherMapResponse>(response);
                // currentWeather = JsonConvert.DeserializeObject<OpenWeatherMapResponse>(
                //     @"{""coord"":{""lon"":-80.14,""lat"":26.12},""weather"":[{""id"":801,""main"":""Clouds"",""description"":""few clouds"",""icon"":""02d""}],""base"":""stations"",""main"":{""temp"":74.3,""pressure"":1020,""humidity"":73,""temp_min"":73.4,""temp_max"":75.2},""visibility"":16093,""wind"":{""speed"":11.41,""deg"":40},""clouds"":{""all"":20},""dt"":1517867580,""sys"":{""type"":1,""id"":657,""message"":0.0035,""country"":""US"",""sunrise"":1517832128,""sunset"":1517872047},""id"":4155966,""name"":""Fort Lauderdale"",""cod"":200}"
                // );

                // Keep in cache for this duration; reset time if accessed
                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromSeconds(
                        weatherConfig.GetValue<int>("CacheDuration"))
                };

                // Save data in cache
                _cache.Set(WEATHER_CACHE_KEY, currentWeather, cacheEntryOptions);
            }

            return currentWeather;
        }
    }
}
