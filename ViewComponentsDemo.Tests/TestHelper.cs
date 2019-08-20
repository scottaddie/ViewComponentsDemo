﻿using Microsoft.Extensions.Configuration;

namespace ViewComponentsDemo.Tests
{
    public static class TestHelper
    {
        public static IConfigurationRoot InitConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.test.json")
                .Build();

            return config;
        }
    }
}
