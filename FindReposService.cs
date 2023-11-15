using Newtonsoft.Json;

using System.Net.Http.Headers;

namespace GitSearch
{
    public class FindReposService
    {
        private HttpClient _client = new HttpClient();

        public async Task<Rootobject?> GetRepos(string url)
        {

            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            _client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("AppName", "1.0"));

            HttpResponseMessage response = await _client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            
            string responseBody = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<Rootobject>(responseBody);

            return result;
        }
    }
}
