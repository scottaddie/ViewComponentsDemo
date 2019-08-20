using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.Configuration;
using Moq;
using ViewComponentsDemo.Mappers;
using ViewComponentsDemo.Models;
using ViewComponentsDemo.Services;
using ViewComponentsDemo.ViewComponents;
using Xunit;

namespace ViewComponentsDemo.Tests.ViewComponents
{
    public class CurrentWeatherTests
    {
        private readonly IConfigurationRoot _configuration;

        public CurrentWeatherTests()
        {
            _configuration = TestHelper.InitConfiguration();
        }

        [Fact]
        public async Task InvokeAsync_Returns_ViewViewComponentResult()
        {
            // Arrange
            IConfigurationSection weatherConfig = _configuration.GetSection("Weather");

            var request = new ForecastRequest
            {
                City = weatherConfig["City"],
                CountryCode = weatherConfig["CountryCode"],
                LanguageCode = Language.French.ToLanguageCode(),
                TemperatureScale = TemperatureScale.Fahrenheit.ToUnitsType(),
            };

            var serviceMock = new Mock<WeatherService>();
            serviceMock.Setup(stub => stub.GetCurrentWeatherAsync(request)).ReturnsAsync(GetTestForecast());

            var viewComponent = new CurrentWeather(serviceMock.Object);

            // Act
            var result = await viewComponent.InvokeAsync(
                weatherConfig["City"], weatherConfig["CountryCode"], TemperatureScale.Fahrenheit, Language.French);

            // Assert
            Assert.IsType<ViewViewComponentResult>(result);
        }

        private Forecast GetTestForecast() =>
            new Forecast
            {
                Main = new Main
                {
                    Humidity = 0,
                    Temp = 0,
                    Temp_Max = 0,
                    Temp_Min = 0,
                },
                Name = "test",
                Sys = new Sys
                {
                    Country = "US",
                },
                Weather = new List<Weather>
                {
                    new Weather
                    {
                        Description = "test",
                        Icon = "test",
                        Main = "test",
                    }
                },
            };
    }
}
