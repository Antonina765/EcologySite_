namespace EcologySite.Services.Apis
{
    public class HttpNumberApi
    {
        public HttpClient _httpClient;

        public HttpNumberApi(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetFactAsync(int number)
        {
            return await _httpClient.GetStringAsync($"{number}");
        }
    }
}
