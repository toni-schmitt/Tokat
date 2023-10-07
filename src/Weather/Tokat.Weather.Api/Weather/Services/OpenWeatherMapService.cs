using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Tokat.Weather.Api.Weather.Services;

[SuppressMessage(
    "ReSharper",
    "ClassNeverInstantiated.Local"
)]
public class OpenWeatherMapService : IWeatherService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly WeatherServiceConfiguration _weatherServiceConfiguration;

    public OpenWeatherMapService(
        IHttpClientFactory httpClientFactory,
        WeatherServiceConfiguration weatherServiceConfiguration
    )
    {
        _httpClientFactory = httpClientFactory;
        _weatherServiceConfiguration = weatherServiceConfiguration;
    }

    public async Task<WeatherResponse?> GetWeatherAsync(string cityName)
    {
        HttpClient client = _httpClientFactory.CreateClient();

        string url = $"{_weatherServiceConfiguration.BaseUrl}/weather"
                     + $"?q={cityName}"
                     + $"&appid={_weatherServiceConfiguration.ApiKey}"
                     + $"&units=metric";

        HttpResponseMessage response = await client.GetAsync(
            url
        );

        if (response.IsSuccessStatusCode is false)
        {
            return null;
        }

        OpenWeatherMapWeatherResponse? openWeatherMapWeatherResponse =
            await response.Content
                .ReadFromJsonAsync<OpenWeatherMapWeatherResponse>();

        if (openWeatherMapWeatherResponse is null)
        {
            return null;
        }

        return new WeatherResponse(
            openWeatherMapWeatherResponse.Weather.Temp,
            openWeatherMapWeatherResponse.Weather.FeelsLike
        );
    }

    [SuppressMessage(
        "ReSharper",
        "UnusedAutoPropertyAccessor.Local"
    )]
    [SuppressMessage(
        "ReSharper",
        "UnusedMember.Local"
    )]
    private class OpenWeatherMapWeatherResponse
    {
        [JsonPropertyName(
            "main"
        )]
        public required OpenWeatherMapWeather Weather { get; set; }
        [JsonPropertyName(
            "visibility"
        )]
        public int Visibility { get; set; }

        [JsonPropertyName(
            "dt"
        )]
        public int Dt { get; set; }

        [JsonPropertyName(
            "timezone"
        )]
        public int Timezone { get; set; }

        [JsonPropertyName(
            "id"
        )]
        public int Id { get; set; }

        [JsonPropertyName(
            "name"
        )]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName(
            "cod"
        )]
        public int Cod { get; set; }
    }

    [SuppressMessage(
        "ReSharper",
        "UnusedAutoPropertyAccessor.Local"
    )]
    [SuppressMessage(
        "ReSharper",
        "UnusedMember.Local"
    )]
    private record OpenWeatherMapWeather
    {

        [JsonPropertyName(
            "temp"
        )]
        public double Temp { get; set; }

        [JsonPropertyName(
            "feels_like"
        )]
        public double FeelsLike { get; set; }

        [JsonPropertyName(
            "temp_min"
        )]
        public double TempMin { get; set; }

        [JsonPropertyName(
            "temp_max"
        )]
        public double TempMax { get; set; }

        [JsonPropertyName(
            "pressure"
        )]
        public int Pressure { get; set; }

        [JsonPropertyName(
            "humidity"
        )]
        public int Humidity { get; set; }
    }
}
