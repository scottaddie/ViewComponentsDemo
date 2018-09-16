using System.Collections.Generic;

namespace ViewComponentsDemo.Models
{
    /// <summary>
    /// A partial representation of a response object from the OpenWeather API
    /// </summary>
    public class OpenWeatherMapResponse
    {
        public List<Weather> Weather { get; set; }
        public Main Main { get; set; }
        public Sys Sys { get; set; }
        public string Name { get; set; }
    }

    public class Weather
    {
        public string Main { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
    }

    public class Sys
    {
        public string Country { get; set; }
    }

    public class Main
    {
        public double Temp { get; set; }
        public double Humidity { get; set; }
        public double Temp_Min { get; set; }
        public double Temp_Max { get; set; }
    }
}
