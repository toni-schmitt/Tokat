using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using NSubstitute;
using Tokat.Weather.Api.Endpoints;
using Tokat.Weather.Api.Services;
using Xunit;

namespace Tokat.Weather.Api.UnitTests.Endpoints;

public class GetEndpointsTests
{
    [Fact]
    public async Task GetWeatherAsync_ReturnsNotFound_On_InvalidCity()
    {
        const string invalidCity = "blahbliblup";

        IWeatherService? substituteForWeatherService =
            Substitute.For<IWeatherService>();

        substituteForWeatherService.When(
            x => x.GetWeatherAsync(
                    invalidCity
                )
                .Returns(
                    Task.FromResult(
                        (WeatherResponse?)null
                    )
                )
        );

        IResult response = await GetEndpoints.GetWeatherAsync(
            invalidCity,
            substituteForWeatherService
        );

        Assert.True(
            response is NotFound
        );
    }

    [Fact]
    public async Task GetWeatherAsync_ReturnsWeather_On_ValidCity()
    {
        const string validCity = "London";

        IWeatherService? substituteForWeatherService =
            Substitute.For<IWeatherService>();

        substituteForWeatherService.When(
            x => x.GetWeatherAsync(
                    validCity
                )
                .Returns(
                    Task.FromResult(
                        (WeatherResponse?)new WeatherResponse(
                            20.5,
                            19.3
                        )
                    )
                )
        );

        IResult response = await GetEndpoints.GetWeatherAsync(
            validCity,
            substituteForWeatherService
        );

        Assert.True(
            response is Ok<WeatherResponse>
        );
    }
}
