using System;
using ViewComponentsDemo.Models;
using VM = ViewComponentsDemo.ViewModels;

namespace ViewComponentsDemo.Mappers
{
    public enum TemperatureScale
    {
        Celsius,
        Fahrenheit
    }

    public static class TemperatureScaleExtensions
    {
        public static string ToFriendlyString(this TemperatureScale tempScale)
        {
            switch(tempScale)
            {
                case TemperatureScale.Celsius:
                    return "C";
                case TemperatureScale.Fahrenheit:
                    return "F";
                default:
                    return string.Empty;
            }
        }
    }

    public static class WeatherMapper
    { 
        public static VM.Weather MapToWeather(this OpenWeatherMapResponse response,
            TemperatureScale tempScale)
        {
            var weather = new VM.Weather
            {
                Humidity = response.Main.Humidity,
                Location = response.Name,
                Scale = tempScale.ToFriendlyString(),
                TemperatureCurrent = Math.Ceiling(response.Main.Temp),
                TemperatureLow = Math.Ceiling(response.Main.Temp_Min),
                TemperatureHigh = Math.Ceiling(response.Main.Temp_Max)
            };

            return weather;
        }
    }
}
