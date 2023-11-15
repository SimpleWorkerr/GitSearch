using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text.Unicode;

namespace GitSearch
{
    public class FindReposMiddleware
    {
        private RequestDelegate _next;
        private string _url;

        public FindReposMiddleware(RequestDelegate next, string url)
        {
            _next = next;
            _url = url;
        }

        public async Task InvokeAsync(HttpContext context, FindReposService repoService)
        {   
            var response = context.Response;
            var request = context.Request;

            JsonSerializerOptions options = new()
            {
                WriteIndented = true
            };

            var fullResult = await repoService.GetRepos(_url);

            var result = from tempValue in fullResult?.items
                         select
                            new
                            {
                                tempValue.id,
                                tempValue.name,
                                tempValue.full_name,
                                tempValue._private,
                                tempValue.owner,
                                tempValue.description,
                                tempValue.url,
                                tempValue.downloads_url,
                                tempValue.clone_url,
                                tempValue.created_at,
                                tempValue.updated_at,
                                tempValue.pushed_at
                            };


            await response.WriteAsJsonAsync(result, options);

        }
    }
}
