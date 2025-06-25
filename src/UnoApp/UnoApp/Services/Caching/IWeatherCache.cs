namespace UnoApp.Services.Caching;
using WeatherForecast = UnoApp.Client.Models.WeatherForecast;
public interface IWeatherCache
{
    ValueTask<IImmutableList<WeatherForecast>> GetForecast(CancellationToken token);
}
