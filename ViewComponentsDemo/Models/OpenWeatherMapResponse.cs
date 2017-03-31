using System.Collections.Generic;

namespace ViewComponentsDemo.Models
{
    public class OpenWeatherMapResponse
    {
        public Coordinates Coord { get; set; }
        public List<Weather> Weather { get; set; }
        public string Base { get; set; }
        public Main Main { get; set; }
        public Wind Wind { get; set; }
        public Clouds Clouds { get; set; }
        public Rain Rain { get; set; }
        public int Dt { get; set; }
        public Sys Sys { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public int Cod { get; set; }
    }

    public class Weather
    {
        public int Id { get; set; }
        public string Main { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
    }

    public class Sys
    {
        public int Type { get; set; }
        public int Id { get; set; }
        public double Message { get; set; }
        public string Country { get; set; }
        public int Sunrise { get; set; }
        public int Sunset { get; set; }
    }

    public class Rain
    {
        public int ThreeH { get; set; }
    }

    public class Clouds
    {
        public double All { get; set; }
    }

    public class Wind
    {
        public double Speed { get; set; }
        public double Deg { get; set; }
    }

    public class Main
    {
        public double Temp { get; set; }
        public double Pressure { get; set; }
        public double Humidity { get; set; }
        public double Temp_Min { get; set; }
        public double Temp_Max { get; set; }
    }

    public class Coordinates
    {
        public double Lon { get; set; }
        public double Lat { get; set; }
    }
}
