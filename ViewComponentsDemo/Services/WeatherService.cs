﻿using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using ViewComponentsDemo.Models;

namespace ViewComponentsDemo.Services
{
    public interface IWeatherService
    {
        Task<Forecast> GetCurrentWeatherAsync(ForecastRequest request);
    }

    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _cache;

        public WeatherService(HttpClient httpClient,
                              IConfiguration configuration,
                              IMemoryCache cache)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _cache = cache;
        }

        public async Task<Forecast> GetCurrentWeatherAsync(ForecastRequest request)
        {
            const string WEATHER_CACHE_KEY = "Weather";

            // Look for cache key
            if (!_cache.TryGetValue(WEATHER_CACHE_KEY, out Forecast currentWeather))
            {
                // Key not in cache, so get data

                // Fetch the user secret
                var apiKey = _configuration.GetValue<string>("OpenWeatherMapApiKey");

                if (String.IsNullOrWhiteSpace(apiKey))
                {
                    throw new ArgumentException("Unable to find an OpenWeatherMap API key in the user secret store.");
                }

                IConfigurationSection weatherConfig = _configuration.GetSection("Weather");
                var baseUrl = weatherConfig.GetValue<string>("ApiBaseUrl");
                var endpointUrl = $"{baseUrl}?q={request.City},{request.CountryCode}&lang={request.LanguageCode}&units={request.TemperatureScale}&appid={apiKey}";

                var response = await _httpClient.GetAsync(endpointUrl);
                response.EnsureSuccessStatusCode();
                currentWeather = await response.Content.ReadAsAsync<Forecast>();

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
