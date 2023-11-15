using System.Text.Json;

namespace GitSearch
{
    public class MainPageMiddleware
    {
        private RequestDelegate _next;

        public MainPageMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if(context.Request.Path == "/")
            {
                context.Response.ContentType = "text/html; charset=utf-8";

                await context.Response.SendFileAsync("C:..\\GitSearch\\wwwroot\\index.html");
            }
            else
            {
                await _next.Invoke(context);    
            }
        }
    }
}
