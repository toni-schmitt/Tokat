namespace Tokat.Weather.Api.Services;

public interface IWeatherService
{
    Task<WeatherResponse?> GetWeatherAsync(string cityName);
}
