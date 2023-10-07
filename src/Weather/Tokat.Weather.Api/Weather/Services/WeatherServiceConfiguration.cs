namespace Tokat.Weather.Api.Weather.Services;

public record WeatherServiceConfiguration
{
    public string Service { get; set; } = string.Empty;
    public required string BaseUrl { get; init; }
    public required string ApiKey { get; init; }
}
