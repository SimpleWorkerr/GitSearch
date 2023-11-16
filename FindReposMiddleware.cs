using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text.Unicode;
using System.Web;
using System.Xml.Linq;
using System.Linq;

namespace GitSearch
{
    public class FindReposMiddleware
    {
        private static (string, string)[] languages =
            {
                ("python","python"),
                ("c++", "c%2B%2B"),
                ("java", "java"),
                ("javascript", "javascript"),
                ("c#", "c%23"),
                ("ruby", "ruby"),
                ("php", "php"),
                ("swift", "swift"),
                ("go", "go"),
                ("kotlin", "kotlin"),
                ("rust", "rust"),
                ("typescript", "typescript"),
                ("perl", "perl"),
                ("c", "c"),
                ("lua", "lua"),
                ("r", "r"),
                ("matlab", "matlab"),
                ("haskerll", "haskerll"),
                ("groovy", "groovy"),
                ("scala", "scala") 
        };

        private RequestDelegate _next;

        public FindReposMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, FindReposService repoService, DataAnalysisService anylysisSevice)
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
                

                string? description = query["description"].ToString().ToLower();
                string langParam = "";

                foreach(var lang in languages)
                {
                    foreach(var param in description.Split(' '))
                    {
                        if(lang.Item1 == param)
                        {
                            description = description.Replace(param, "");
                            langParam += $"+language:{lang.Item2}";
                        }
                            
                    }
                }

                string url = $"https://api.github.com/search/repositories?q={query["name"]}" + langParam + "&per_page=100";
                string url_topic = $"https://api.github.com/search/repositories?q={query["name"]} {description}+in:topic" + langParam + "&per_page=100"; ;
                string url_name = $"https://api.github.com/search/repositories?q={query["name"]} {description}+in:name" + langParam + "&per_page=100"; ;
                string url_description = $"https://api.github.com/search/repositories?q={query["name"]} {description}+in:description" + langParam + "&per_page=100"; ;

                var fullResultUrl = (await repoService.GetRepos(url));
                var fullResultUrl_topic = (await repoService.GetRepos(url_topic));
                var fullResultUrl_name = (await repoService.GetRepos(url_name));
                var fullResultUrl_description = (await repoService.GetRepos(url_description));

                var analysResult = await anylysisSevice.GetData(fullResultUrl?.items?.Concat(fullResultUrl_topic?.items)?.Concat(fullResultUrl_name?.items)?.Concat(fullResultUrl_description?.items).ToList(), "");

                var result = from tempValue in analysResult
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
