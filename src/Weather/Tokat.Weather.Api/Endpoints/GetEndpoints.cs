using Microsoft.AspNetCore.Mvc;
using Tokat.Weather.Api.Services;

namespace Tokat.Weather.Api.Endpoints;

public static class GetEndpoints
{
    public static IEndpointRouteBuilder MapGetEndpoints(
        this IEndpointRouteBuilder endpoints
    )
    {
        endpoints.MapGet(
                "/weather/{{cityName}}",
                (
                    [FromRoute] string cityName,
                    [FromServices] IWeatherService service
                ) => GetWeatherAsync(
                    cityName,
                    service
                )
            )
            .Produces<WeatherResponse>()
            .Produces<NotFoundResult>(
                StatusCodes.Status404NotFound
            )
            .WithName(
                "GetWeatherForecastForCity"
            )
            .WithDescription(
                "Get the weather forecast for a given city."
            )
            .WithOpenApi();

        return endpoints;
    }

    internal static async Task<IResult> GetWeatherAsync(
        string cityName,
        IWeatherService weatherService
    )
    {
        WeatherResponse? weather = await weatherService.GetWeatherAsync(
            cityName
        );

        if (weather is null)
        {
            return Results.NotFound();
        }

        return Results.Ok(
            weather
        );
    }
}
