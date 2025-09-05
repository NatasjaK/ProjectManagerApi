using Microsoft.AspNetCore.Http;

namespace ProjectManagerApi.Security
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private const string HeaderName = "X-API-KEY";

        public ApiKeyMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext context, IConfiguration config, IWebHostEnvironment env)
        {
            if (HttpMethods.Options.Equals(context.Request.Method, StringComparison.OrdinalIgnoreCase))
            {
                await _next(context);   
                return;
            }

            var path = context.Request.Path;
            if (path == "/" ||
                path.StartsWithSegments("/swagger") ||
                path.StartsWithSegments("/openapi") ||
                path.StartsWithSegments("/scalar") ||
               (path.StartsWithSegments("/uploads") &&
                (HttpMethods.Get.Equals(context.Request.Method, StringComparison.OrdinalIgnoreCase) ||
                HttpMethods.Head.Equals(context.Request.Method, StringComparison.OrdinalIgnoreCase))))
            {
                await _next(context);
                return;
            }

            var configuredKey = config["ApiKey"];
            if (string.IsNullOrWhiteSpace(configuredKey))
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync("API key not configured.");
                return;
            }

            const string HeaderName = "X-API-KEY";
            if (!context.Request.Headers.TryGetValue(HeaderName, out var provided) ||
                !string.Equals(provided, configuredKey, StringComparison.Ordinal))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Invalid or missing API key.");
                return;
            }

            await _next(context);
        }

    }

    public static class ApiKeyMiddlewareExtensions
    {
        public static IApplicationBuilder UseApiKey(this IApplicationBuilder app)
            => app.UseMiddleware<ApiKeyMiddleware>();
    }
}
