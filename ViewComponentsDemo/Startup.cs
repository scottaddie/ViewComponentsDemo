using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using ViewComponentsDemo.Services;

namespace ViewComponentsDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => 
            Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();

            // Failed requests are retried up to 3 times.
            services.AddHttpClient("WeatherApi")
                    .AddTransientHttpErrorPolicy(p => p.RetryAsync(3));

            services.AddMvc()
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Register IConfiguration with DI system to support IConfiguration.GetValue approach
            services.AddSingleton(Configuration);

            // Configure DI to allow for constructor injection
            services.AddTransient<IWeatherService, WeatherService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }
}