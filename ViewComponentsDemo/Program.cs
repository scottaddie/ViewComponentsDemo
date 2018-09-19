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
                       var keyVaultEndpoint = GetKeyVaultEndpoint();

                       // If the environment variable isn't set, it means we're running locally.
                       // In that case, use the .NET Core Secret Manager tool to retrieve secrets.
                       if (!String.IsNullOrEmpty(keyVaultEndpoint))
                       {
                           var azureServiceTokenProvider = new AzureServiceTokenProvider();
                           var keyVaultClient = new KeyVaultClient(
                               new KeyVaultClient.AuthenticationCallback(
                                   azureServiceTokenProvider.KeyVaultTokenCallback));

                           config.AddAzureKeyVault(
                               keyVaultEndpoint, keyVaultClient, new DefaultKeyVaultSecretManager());
                       }
                   })
                   .UseStartup<Startup>();

        private static string GetKeyVaultEndpoint() => 
            Environment.GetEnvironmentVariable("KEYVAULT_ENDPOINT");
    }
}
