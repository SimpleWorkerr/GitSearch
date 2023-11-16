using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text.Unicode;
using System.Web;
using System.Xml.Linq;

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

                string url = $"https://api.github.com/search/repositories?q={query["name"]} {description}+in:name,topic,description";

                url = url + langParam + "&per_page=100";

                Console.WriteLine(url);
                Console.WriteLine(description);

                var fullResult = await repoService.GetRepos(url);

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
