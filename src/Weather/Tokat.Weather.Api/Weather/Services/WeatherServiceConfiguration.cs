namespace Tokat.Weather.Api.Weather.Services;

public record WeatherServiceConfiguration
{
    public required string BaseUrl { get; init; }
    public required string ApiKey { get; init; }
}
