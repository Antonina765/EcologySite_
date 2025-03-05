namespace MultyTreadConsole
{
    internal class ApiService
    {
        public async Task GetDataFromAsync(int number)
        {
            var url = $"http://numbersapi.com/{number}/math";
            HttpClient client = new HttpClient();
            var response = await client.GetStringAsync(url);

            Console.WriteLine(response);
        }
    }
}
