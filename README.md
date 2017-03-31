# ViewComponentsDemo
An ASP.NET Core MVC application demonstrating the use of view components

### Setup
1. Request a free API key for OpenWeatherMap's Current Weather API [here](https://home.openweathermap.org/users/sign_up).
1. Store the API key as a user secret by doing the following:
    * In a command shell, navigate to the folder containing the `*.csproj` file
    * Execute the following command, where *<your_api_key_here>* is replaced with your unique API key:
      ```
      dotnet user-secrets set OpenWeatherMapApiKey <your_api_key_here>
      ```
