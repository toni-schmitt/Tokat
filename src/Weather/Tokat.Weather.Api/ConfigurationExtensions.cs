using Tokat.Weather.Api.Services;

// ReSharper disable once CheckNamespace - this namespace is intended
namespace Microsoft.Extensions.Configuration;

public static class ConfigurationExtensions
{
    public static WeatherServiceConfiguration
        GetWeatherServiceConfiguration<TServiceImplementation>(
            this IConfiguration configuration
        )
        where TServiceImplementation : class, IWeatherService
    {
        Type serviceImplementationType = typeof(TServiceImplementation);

        string configurationKey = serviceImplementationType.Name
            .Replace(
                "Service",
                "Configuration"
            );

        WeatherServiceConfiguration? weatherServiceConfiguration = configuration
            .GetSection(
                configurationKey
            )
            .Get<WeatherServiceConfiguration>();

        if (weatherServiceConfiguration is null)
        {
            throw new InvalidOperationException(
                $"Could not find configuration {nameof(WeatherServiceConfiguration)} for {configurationKey}"
            );
        }

        weatherServiceConfiguration.Service = serviceImplementationType.Name;

        return weatherServiceConfiguration;
    }
}
