namespace Vessel_Tracking_Api.Middleware
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _config;

        public ApiKeyMiddleware(RequestDelegate next, IConfiguration config)
        {
            _next = next;
            _config = config;
        }

        /// <summary>
        /// Middleware that validates the API key for token generation requests.
        /// </summary>
        /// <remarks>
        /// This middleware intercepts requests to the token endpoint <c>/api/v1/auth/token</c>.
        /// It checks if the request contains a valid <c>X-API-KEY</c> header matching the configured API key.
        /// If the header is missing or invalid, the middleware returns <c>401 Unauthorized</c>.
        /// Otherwise, it passes the request to the next middleware in the pipeline.
        /// </remarks>
        /// <param name="context">The current HTTP context.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/api/v1/auth/token"))
            {
                if (!context.Request.Headers.TryGetValue("X-API-KEY", out var extractedKey))
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("API Key missing");
                    return;
                }

                var apiKey = _config["ApiSecurity:ApiKey"];

                if (!apiKey.Equals(extractedKey))
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Invalid API Key");
                    return;
                }
            }

            await _next(context);
        }
    }
}
