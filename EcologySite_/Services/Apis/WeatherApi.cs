namespace EcologySite.Services.Apis;

public class WeatherApi
{
    private readonly HttpClient _httpClient;

    public WeatherApi(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GetWeatherAsync(double lat, double lon)
    {
        var response = await _httpClient.GetStringAsync($"https://api.open-meteo.com/v1/forecast?latitude={lat}&longitude={lon}&current=temperature_2m,wind_speed_10m&hourly=temperature_2m,relative_humidity_2m,wind_speed_10m");
        return response;
    }
}