using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace TicketDepot.Shared
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionHandlingMiddleware> logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await this.next(context);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Unhandled exception occurred.");

                context.Response.StatusCode = ex switch
                {
                    ArgumentException => StatusCodes.Status400BadRequest,
                    KeyNotFoundException => StatusCodes.Status404NotFound,
                    UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                    _ => StatusCodes.Status500InternalServerError
                };

                context.Response.ContentType = "application/json";
                string result = JsonSerializer.Serialize(new
                {
                    error = ex.Message,
                    traceId = context.TraceIdentifier // Optional for debugging
                });
                await context.Response.WriteAsync(result);
            }
        }
    }

}
