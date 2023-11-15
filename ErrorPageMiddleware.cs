namespace GitSearch
{
    public class ErrorPageMiddleware
    {
        private RequestDelegate _next;

        public ErrorPageMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Response.ContentType = "text/html; charset=utf-8";

            await context.Response.SendFileAsync("C:..\\GitSearch\\wwwroot\\html\\error-page.html");
        }
    }
}
