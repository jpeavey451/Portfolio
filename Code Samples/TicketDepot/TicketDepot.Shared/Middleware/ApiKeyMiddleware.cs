using Microsoft.AspNetCore.Http;
using System.Net;

namespace TicketDepot.Shared.Middleware
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IApiKeyValidation apiKeyValidation;

        public ApiKeyMiddleware(RequestDelegate next, IApiKeyValidation apiKeyValidation)
        {
            this.next = next;
            this.apiKeyValidation = apiKeyValidation;
        }
         public async Task InvokeAsync(HttpContext context)
        {
            string apiKeyHeader = context.Request.Headers[AuthConfig.ApiKeyHeaderName].ToString();

            if (string.IsNullOrWhiteSpace(apiKeyHeader))
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return;
            }

            if (!apiKeyValidation.IsValidApiKey(apiKeyHeader))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            }

            await this.next(context);
        }
    }
}
