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

        public FindReposMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, FindReposService repoService)
        {
            var response = context.Response;
            var request = context.Request;
            var query = request.Query;

            JsonSerializerOptions options = new()
            {
                WriteIndented = true
            };

            if (request.Path == "/getRepoByArgs")
            {
                var fullResult = await repoService.GetRepos($"https://api.github.com/search/repositories?q={query["description"]}+in:description+{query["name"]}+in:name");

                var result = from tempValue in fullResult?.items
                             select
                                new
                                {
                                    //tempValue.id,
                                    //tempValue.name,
                                    tempValue.full_name,
                                    //tempValue._private,
                                    //tempValue.owner,
                                    //tempValue.description,
                                    //tempValue.url,
                                    //tempValue.downloads_url,
                                    //tempValue.clone_url,
                                    tempValue.html_url,
                                    tempValue.created_at,
                                    tempValue.updated_at,
                                    //tempValue.pushed_at                                    
                                    //tempValue.name,
                                    tempValue.visibility,
                                    tempValue.language,                                                                
                                    tempValue.watchers_count,
                                    tempValue.forks_count,
                                    tempValue.owner?.avatar_url
                                };


                await response.WriteAsJsonAsync(result, options);
            }
            
            else
                await _next.Invoke(context);
        }
    }
}
