using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text.Unicode;
using System.Web;

namespace GitSearch
{
    public class FindReposMiddleware
    {
        private static string[] languages = { "python", "c++", "java", "javascript", "c#", "ruby", "php", "swift", "go", "kotlin", "rust", "typescript", "perl", "c", "lua", "r", "matlab", "haskerll", "groovy", "scala" };

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
                string url = $"https://api.github.com/search/repositories?q={query["description"]}+in:description+{query["name"]}";

                string? description = query["description"].ToString().ToLower();
                string langParam = "";

                foreach(var lang in languages)
                {
                    if (description.Contains(lang))
                    {
                        langParam += $"+language:{lang.Replace("+", "%2B").Replace("#", "%23")}";
                    }
                }

                var fullResult = await repoService.GetRepos(url + langParam + "&per_page=100");

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
