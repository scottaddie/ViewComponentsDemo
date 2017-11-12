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
            // Convert Kelvin temps to desired scale, then round afterwards
            Temp currentTemp = Temp.FromKelvin(response.Main.Temp);
            Temp lowTemp = Temp.FromKelvin(response.Main.Temp_Min);
            Temp highTemp = Temp.FromKelvin(response.Main.Temp_Max);

            var weather = new VM.Weather
            {
                Humidity = response.Main.Humidity,
                Location = response.Name,
                Scale = tempScale.ToFriendlyString(),
                TemperatureCurrent = (tempScale == TemperatureScale.Celsius) ? 
                    Math.Ceiling(currentTemp.Celsius) : Math.Ceiling(currentTemp.Fahrenheit),
                TemperatureLow = (tempScale == TemperatureScale.Celsius) ? 
                    Math.Ceiling(lowTemp.Celsius) : Math.Ceiling(lowTemp.Fahrenheit),
                TemperatureHigh = (tempScale == TemperatureScale.Celsius) ? 
                    Math.Ceiling(highTemp.Celsius) : Math.Ceiling(highTemp.Fahrenheit)
            };

            return weather;
        }
    }

    /// <summary>
    /// Temperature class that uses a base unit of Celsius
    /// </summary>
    public struct Temp
    {
        public static Temp FromCelsius(double value) => 
            new Temp(value);

        public static Temp FromFahrenheit(double value) => 
            new Temp((value - 32) * 5 / 9);

        public static Temp FromKelvin(double value) => 
            new Temp(value - 273.15);

        public static Temp operator +(Temp left, Temp right)
        {
            return Temp.FromCelsius(left.Celsius + right.Celsius);
        }

        private double _value;

        private Temp(double value)
        {
            _value = value;
        }

        public double Kelvin
        {
            get { return _value + 273.15; }
        }

        public double Celsius
        {
            get { return _value; }
        }

        public double Fahrenheit
        {
            get { return _value / 5 * 9 + 32; }
        }
    }
}
