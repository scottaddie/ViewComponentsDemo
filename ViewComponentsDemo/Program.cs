using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;

namespace ViewComponentsDemo
{
    public class Program
    {
        public static void Main(string[] args) =>
            CreateWebHostBuilder(args).Build().Run();

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                   .ConfigureAppConfiguration((context, config) =>
                   {
                       ConfigureKeyVault(ref config);
                   })
                   .UseStartup<Startup>();

        private static void ConfigureKeyVault(ref IConfigurationBuilder config)
        {
            bool.TryParse(Environment.GetEnvironmentVariable(
                "ASPNETCORE_HOSTINGSTARTUP__KEYVAULT__CONFIGURATIONENABLED"),
                out bool isKeyVaultEnabled);

            // If the environment variable is false, it means we're running locally.
            // In that case, use the .NET Core Secret Manager tool to retrieve secrets.
            if (isKeyVaultEnabled)
            {
                var azureServiceTokenProvider = new AzureServiceTokenProvider();
                var keyVaultClient = new KeyVaultClient(
                    new KeyVaultClient.AuthenticationCallback(
                        azureServiceTokenProvider.KeyVaultTokenCallback));

                string keyVaultEndpoint = Environment.GetEnvironmentVariable(
                    "ASPNETCORE_HOSTINGSTARTUP__KEYVAULT__CONFIGURATIONVAULT");

                config.AddAzureKeyVault(
                    keyVaultEndpoint, keyVaultClient, new DefaultKeyVaultSecretManager());
            }
        }
    }
}
