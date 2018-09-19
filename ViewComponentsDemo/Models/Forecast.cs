using System.Collections.Generic;
using Newtonsoft.Json;

namespace ViewComponentsDemo.Models
{
    /// <summary>
    /// A partial representation of a response object from the OpenWeather API
    /// </summary>
    public class Forecast
    {
        public Forecast()
        {
        }

        [JsonProperty(PropertyName = "weather")]
        public List<Weather> Weather { get; set; }
        [JsonProperty(PropertyName = "main")]
        public Main Main { get; set; }
        [JsonProperty(PropertyName = "sys")]
        public Sys Sys { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }

    public class Weather
    {
        [JsonProperty(PropertyName = "main")]
        public string Main { get; set; }
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "icon")]
        public string Icon { get; set; }
    }

    public class Sys
    {
        [JsonProperty(PropertyName = "country")]
        public string Country { get; set; }
    }

    public class Main
    {
        [JsonProperty(PropertyName = "temp")]
        public double Temp { get; set; }
        [JsonProperty(PropertyName = "humidity")]
        public double Humidity { get; set; }
        [JsonProperty(PropertyName = "temp_min")]
        public double Temp_Min { get; set; }
        [JsonProperty(PropertyName = "temp_max")]
        public double Temp_Max { get; set; }
    }
}
