using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ViewComponents;
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
        [Fact]
        public async Task InvokeAsync_Returns_ViewViewComponentResult()
        {
            // Arrange
            var forecastRequest = new ForecastRequest
            {
                City = "St. Louis",
                CountryCode = "US",
                LanguageCode = Language.French.ToLanguageCode(),
                TemperatureScale = TemperatureScale.Fahrenheit.ToUnitsType()
            };

            var weatherServiceMock = new Mock<IWeatherService>();
            weatherServiceMock.Setup(stub => stub.GetCurrentWeatherAsync(forecastRequest)).ReturnsAsync(GetTestForecast());
            // The following line is equivalent to the previous line:
            //weatherServiceMock.Setup(stub => stub.GetCurrentWeatherAsync(forecastRequest)).Returns(Task.FromResult(currentWeather));

            var viewComponent = new CurrentWeather(weatherServiceMock.Object);

            // Act
            var result = await viewComponent.InvokeAsync("St. Louis", "US", TemperatureScale.Fahrenheit, Language.French);

            // Assert
            Assert.IsType<ViewViewComponentResult>(result);
        }

        private Forecast GetTestForecast()
        {
            var currentWeather = new Forecast
            {
                Main = new Main
                {
                    Humidity = 0,
                    Temp = 0,
                    Temp_Max = 0,
                    Temp_Min = 0
                },
                Name = "test",
                Sys = new Sys
                {
                    Country = "US"
                },
                Weather = new List<Weather>
                {
                    new Weather
                    {
                        Description = "test",
                        Icon = "test",
                        Main = "test"
                    }
                }
            };

            return currentWeather;
        }
    }
}
