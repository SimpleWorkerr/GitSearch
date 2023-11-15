namespace GitSearch
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            app.UseStaticFiles();

            app.Run(async (HttpContext context) =>
            {
                HttpResponse response = context.Response;
                HttpRequest request = context.Request;

                await response.SendFileAsync("C:..\\GitSearch\\wwwroot\\index.html");
            });

            app.Run();
        }
    }
}