using JCT_Tracking_Api.Models;

namespace JCT_Tracking_Api.Services
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                LogError(context, ex, "Validation Error");
                await HandleException(context, StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (UnauthorizedException ex)
            {
                LogError(context, ex, "Unauthorized Error");
                await HandleException(context, StatusCodes.Status401Unauthorized, ex.Message);
            }
            catch (ForbiddenException ex)
            {
                LogError(context, ex, "Forbidden Error");
                await HandleException(context, StatusCodes.Status403Forbidden, ex.Message);
            }
            catch (NotFoundException ex)
            {
                LogError(context, ex, "Not Found Error");
                await HandleException(context, StatusCodes.Status404NotFound, ex.Message);
            }
            catch (Exception ex)
            {
                LogError(context, ex, "Unhandled Exception");
                await HandleException(context, StatusCodes.Status500InternalServerError, "Internal Server Error.");
            }
        }

        private void LogError(HttpContext context, Exception ex, string message)
        {
            var endpoint = context.GetEndpoint()?.DisplayName;

            _logger.LogError(ex,
                "{Message}. Method: {Method}, Path: {Path}, Endpoint: {Endpoint}",
                message,
                context.Request.Method,
                context.Request.Path,
                endpoint);
        }

        private static async Task HandleException(HttpContext context, int statusCode, string message)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(new
            {
                success = false,
                message = message,
                data = (object)null
            });
        }
    }
}