using System;
using ViewComponentsDemo.Models;
using VM = ViewComponentsDemo.ViewModels;

namespace ViewComponentsDemo.Mappers
{
    public enum Language
    {
        Bulgarian,
        English,
        French
    }

    public static class LanguageExtensions
    {
        public static string ToLanguageCode(this Language lang)
        {
            switch (lang)
            {
                case Language.Bulgarian:
                    return "bg";
                case Language.English:
                    return "en";
                case Language.French:
                    return "fr";
                default:
                    return "en";
            }
        }
    }

    public enum TemperatureScale
    {
        Celsius,
        Fahrenheit
    }

    public static class TemperatureScaleExtensions
    {
        public static string ToFriendlyString(this TemperatureScale tempScale)
        {
            switch (tempScale)
            {
                case TemperatureScale.Celsius:
                    return "C";
                case TemperatureScale.Fahrenheit:
                    return "F";
                default:
                    return string.Empty;
            }
        }

        public static string ToUnitsType(this TemperatureScale tempScale)
        {
            switch (tempScale)
            {
                case TemperatureScale.Celsius:
                    return "metric";
                case TemperatureScale.Fahrenheit:
                    return "imperial";
                default:
                    return string.Empty;
            }
        }
    }

    public static class WeatherMapper
    {
        public static VM.Weather MapToWeather(this Forecast response,
                                              TemperatureScale tempScale)
        {
            var conditions = string.Empty;
            var iconMarkup = string.Empty;

            response.Weather?.ForEach(c =>
            {
                iconMarkup = string.Empty;

                if (!string.IsNullOrEmpty(c.Icon))
                {
                    iconMarkup = $"<img src='http://openweathermap.org/img/w/{c.Icon}.png' alt='Icon depicting current weather' />";
                }

                conditions += $"{c.Main} ( {c.Description} ) {iconMarkup}<br />";
            });

            var weather = new VM.Weather
            {
                Conditions = conditions,
                Humidity = response.Main.Humidity,
                Location = $"{response.Name}, {response.Sys?.Country}",
                Scale = tempScale.ToFriendlyString(),
                TemperatureCurrent = Math.Ceiling(response.Main.Temp),
                TemperatureLow = Math.Ceiling(response.Main.Temp_Min),
                TemperatureHigh = Math.Ceiling(response.Main.Temp_Max)
            };

            return weather;
        }
    }
}
