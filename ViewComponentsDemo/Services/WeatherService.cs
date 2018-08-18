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
                var response = await client.GetAsync(endpointUrl);

                if (response.IsSuccessStatusCode)
                {
                    var responseAsString = await response.Content.ReadAsStringAsync();

                    currentWeather = JsonConvert.DeserializeObject<OpenWeatherMapResponse>(responseAsString);
                    //currentWeather = JsonConvert.DeserializeObject<OpenWeatherMapResponse>(
                    //    @"{""coord"":{""lon"":-94.56,""lat"":39.08},""weather"":[{""id"":803,""main"":""Clouds"",""description"":""nuageux"",""icon"":""04d""}],""base"":""stations"",""main"":{""temp"":98.02,""pressure"":1017,""humidity"":39,""temp_min"":95,""temp_max"":102.2},""visibility"":16093,""wind"":{""speed"":6.62,""deg"":173.507},""clouds"":{""all"":75},""dt"":1531422000,""sys"":{""type"":1,""id"":1647,""message"":0.0049,""country"":""US"",""sunrise"":1531393391,""sunset"":1531446269},""id"":4393217,""name"":""Kansas City"",""cod"":200}"
                    //);

                    // Keep in cache for this duration; reset time if accessed
                    var cacheEntryOptions = new MemoryCacheEntryOptions
                    {
                        SlidingExpiration = TimeSpan.FromSeconds(
                            weatherConfig.GetValue<int>("CacheDuration"))
                    };

                    // Save data in cache
                    _cache.Set(WEATHER_CACHE_KEY, currentWeather, cacheEntryOptions);
                }
                else
                {
                    currentWeather = null;
                }
            }

            return currentWeather;
        }
    }
}
