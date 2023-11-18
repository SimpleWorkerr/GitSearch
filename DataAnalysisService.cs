using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GitSearch
{
    public class DataAnalysisService
    {
        private HttpClient _client = new HttpClient();

        public async Task<Item[]?> GetData(object items, string url)
        {
            using var response = await _client.PostAsJsonAsync(url, items);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                Console.WriteLine("Ошибка");
            }

            Item[]? resItems = await response.Content.ReadFromJsonAsync<Item[]?>();

            return resItems;
        }
    }
}
