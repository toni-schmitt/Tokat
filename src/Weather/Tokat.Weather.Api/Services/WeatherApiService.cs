using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Tokat.Weather.Api.Services;

[SuppressMessage(
    "ReSharper",
    "ClassNeverInstantiated.Local"
)]
public class WeatherApiService : IWeatherService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly WeatherServiceConfiguration _weatherServiceConfiguration;

    public WeatherApiService(
        IHttpClientFactory httpClientFactory,
        IEnumerable<WeatherServiceConfiguration> weatherServiceConfigurations
    )
    {
        _httpClientFactory = httpClientFactory;

        _weatherServiceConfiguration = weatherServiceConfigurations
            .Single(
                x => x.Service
                     == GetType()
                         .Name
            );
    }

    public async Task<WeatherResponse?> GetWeatherAsync(string cityName)
    {
        HttpClient client = _httpClientFactory.CreateClient();

        string url = $"{_weatherServiceConfiguration.BaseUrl}/current.json"
                     + $"?key={_weatherServiceConfiguration.ApiKey}"
                     + $"&q={cityName}"
                     + $"&aqi=no";

        HttpResponseMessage response = await client.GetAsync(
            url
        );

        if (response.IsSuccessStatusCode is false)
        {
            return null;
        }

        WeatherApiWeatherResponse? weatherApiResponse = await response.Content
            .ReadFromJsonAsync<WeatherApiWeatherResponse>();

        if (weatherApiResponse is null)
        {
            return null;
        }

        return new WeatherResponse(
            weatherApiResponse.Current.TemperatureC,
            weatherApiResponse.Current.FeelsLikeC
        );
    }

    [SuppressMessage(
        "ReSharper",
        "UnusedAutoPropertyAccessor.Local"
    )]
    private class WeatherApiWeatherResponse
    {
        [JsonPropertyName(
            "current"
        )]
        public WeatherApiWeatherCurrent Current { get; } = null!;
    }

    [SuppressMessage(
        "ReSharper",
        "UnusedAutoPropertyAccessor.Local"
    )]
    private class WeatherApiWeatherCurrent
    {
        [JsonPropertyName(
            "temp_c"
        )]
        public double TemperatureC { get; set; }
        [JsonPropertyName(
            "feelslike_c"
        )]
        public double FeelsLikeC { get; set; }
    }
}
