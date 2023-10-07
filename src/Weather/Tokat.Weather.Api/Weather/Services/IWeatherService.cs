namespace Tokat.Weather.Api.Weather.Services;

public interface IWeatherService
{
    Task<WeatherResponse?> GetWeatherAsync(string cityName);
}
