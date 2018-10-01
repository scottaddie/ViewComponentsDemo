# ViewComponentsDemo

[![Build Status](https://scottaddie.visualstudio.com/ViewComponentsDemo/_apis/build/status/scottaddie.ViewComponentsDemo)](https://scottaddie.visualstudio.com/ViewComponentsDemo/_build/latest?definitionId=4)

An ASP.NET Core MVC app demonstrating the use of:
* [View Components](https://docs.microsoft.com/aspnet/core/mvc/views/view-components)
* [Azure Key Vault](https://docs.microsoft.com/azure/key-vault/)
* [IHttpClientFactory](https://docs.microsoft.com/aspnet/core/fundamentals/http-requests)
* [Azure Pipelines](https://docs.microsoft.com/azure/devops/pipelines/languages/dotnet-core)

### Prerequisites

1. .NET Core SDK 2.1 or later. ([Download here](https://www.microsoft.com/net/download/all))

### Setup

1. Request a free API key for OpenWeatherMap's Current Weather API [here](https://home.openweathermap.org/users/sign_up).
1. Store the API key as a user secret by doing the following:
    * In a command shell, navigate to the folder containing the `*.csproj` file
    * Execute the following command, where *<your_api_key_here>* is replaced with your unique API key:
      ```
      dotnet user-secrets set OpenWeatherMapApiKey <your_api_key_here>
      ```
