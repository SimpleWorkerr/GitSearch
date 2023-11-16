using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace GitSearch
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddTransient<FindReposService>();
            builder.Services.AddTransient<DataAnalysisService>();

            var app = builder.Build();

            app.UseStaticFiles();

            app.UseMiddleware<MainPageMiddleware>();
            app.UseMiddleware<FindReposMiddleware>();
            app.UseMiddleware<ErrorPageMiddleware>();
            app.Run();
        }
    }
}