namespace GitSearch
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddTransient<FindReposService>();

            var app = builder.Build();

            app.UseStaticFiles();

            app.UseMiddleware<FindReposMiddleware>("https://api.github.com/search/repositories?q=asp.net+in:description+example+in:name");

            app.Run();
        }
    }
}