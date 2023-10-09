using Tokat.Weather.Api.Endpoints;
using Tokat.Weather.Api.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(
    args
);

builder.Configuration.AddIniFile(
    "env.ini",
    true,
    true
);

builder.Services.AddSingleton(
    builder.Configuration
        .GetWeatherServiceConfiguration<OpenWeatherMapService>()
);

builder.Services.AddSingleton(
    builder.Configuration.GetWeatherServiceConfiguration<WeatherApiService>()
);

builder.Services.AddSingleton<IWeatherService, OpenWeatherMapService>();
builder.Services.AddSingleton<IWeatherService, WeatherApiService>();
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

app.MapGetEndpoints();

app.Run();
