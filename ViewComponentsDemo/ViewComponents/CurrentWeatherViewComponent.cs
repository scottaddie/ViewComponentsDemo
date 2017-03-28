using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ViewComponentsDemo.ViewModels;

namespace ViewComponentsDemo.ViewComponents
{
    public class CurrentWeatherViewComponent : ViewComponent
    {
        private readonly IConfiguration _configuration;

        public CurrentWeatherViewComponent(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //TODO: See async/await examples here: 
        //http://www.csharpstar.com/async-await-keyword-csharp/
        //https://msdn.microsoft.com/en-us/library/mt674882.aspx
        public async Task<IViewComponentResult> InvokeAsync(int maxPriority, bool isDone)
        {
            var weather = await GetWeatherAsync(maxPriority, isDone);

            return View(weather);
        }

        private Task<OpenWeatherMapResponse> GetWeatherAsync(int maxPriority, bool isDone)
        {
            // Fetch the user secret
            string apiKey = _configuration.GetValue<string>("OpenWeatherMapApiKey");

            OpenWeatherMapResponse currentWeather = null;

            using (var client = new HttpClient())
            {
                var endpointUrl = $"http://api.openweathermap.org/data/2.5/weather?q=Chicago,il&appid={apiKey}";
                var response = client.GetStringAsync(endpointUrl).Result;
                
                currentWeather = JsonConvert.DeserializeObject<OpenWeatherMapResponse>(response);
            }

            // Convert Kelvin temps to Fahrenheit
            Temp currentTemp = Temp.FromKelvin(currentWeather.Main.Temp);
            Temp lowTemp = Temp.FromKelvin(currentWeather.Main.Temp_Min);
            Temp highTemp = Temp.FromKelvin(currentWeather.Main.Temp_Max);

            // Assign Fahrenheit temps
            currentWeather.Main.Temp = currentTemp.Fahrenheit;
            currentWeather.Main.Temp_Min = lowTemp.Fahrenheit;
            currentWeather.Main.Temp_Max = highTemp.Fahrenheit;

            return Task.Run(() => { return currentWeather; });
        }
    }

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

    /// <summary>
    /// Temperature class that uses a base unit of Celsius
    /// </summary>
    public struct Temp
    {
        public static Temp FromCelsius(double value)
        {
            return new Temp(value);
        }

        public static Temp FromFahrenheit(double value)
        {
            return new Temp((value - 32) * 5 / 9);
        }

        public static Temp FromKelvin(double value)
        {
            return new Temp(value - 273.15);
        }

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
