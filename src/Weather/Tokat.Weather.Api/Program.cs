using Microsoft.AspNetCore.Mvc;
using Tokat.Weather.Api.Weather;
using Tokat.Weather.Api.Weather.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(
    args
);

builder.Configuration.AddIniFile(
    "env.ini",
    true,
    true
);

WeatherServiceConfiguration? openWeatherMapServiceConfiguration = builder
    .Configuration.GetSection(
        "OpenWeatherMapConfiguration"
    )
    .Get<WeatherServiceConfiguration>();

if (openWeatherMapServiceConfiguration is null)
{
    throw new InvalidOperationException(
        "OpenWeatherMapService configuration is missing."
    );
}

builder.Services.AddSingleton(
    openWeatherMapServiceConfiguration
);

builder.Services.AddSingleton<IWeatherService, OpenWeatherMapService>();
builder.Services.AddHttpClient();
builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapGet(
        "/weather/{city}",
        async ([FromRoute] string city, IWeatherService weatherService) =>
        {
            WeatherResponse? weather = await weatherService.GetWeatherAsync(
                city
            );

            // ReSharper disable once ConvertIfStatementToReturnStatement - happy path always at the end :)
            if (weather is null)
            {
                return Results.NotFound();
            }

            return Results.Ok(
                weather
            );
        }
    )
    .WithName(
        "GetWeatherForecastForCity"
    )
    .WithDescription(
        "Get the weather forecast for a given city."
    )
    .WithOpenApi();

app.Run();
