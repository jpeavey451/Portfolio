

namespace Ticket.Depot
{
    using System;
    using System.Net;
    using System.Security.Authentication;
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Cosmos;
    using Microsoft.Extensions.Logging;
    using TicketDepot.Shared;

    /// <summary>
    /// This class handles all Errors for the API.
    /// </summary>
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : ControllerBase
    {
        private readonly ILogger<ErrorController> fallbackLogger;
        private IObjectResultProvider objectResultProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorController"/> class.
        /// </summary>
        /// <param name="fallbackLogger">The logger <see cref="ILogger{T}"/>.</param>
        /// <param name="objectResultProvider"></param>
        public ErrorController(
            ILogger<ErrorController> fallbackLogger,
            IObjectResultProvider objectResultProvider)
            : base()
        {
            this.fallbackLogger = fallbackLogger;
            this.objectResultProvider = objectResultProvider;
        }

        /// <summary>
        /// Captures the errors.
        /// </summary>
        /// <returns><see cref="IActionResult"/>.</returns>
        [Route("error")]
        public IActionResult Error()
        {
            IExceptionHandlerFeature exceptionHandlerFeature = HttpContext.Features.Get<IExceptionHandlerFeature>()!;
            if (exceptionHandlerFeature != null && exceptionHandlerFeature.Error != null)
            {
                Exception ex = exceptionHandlerFeature.Error;
                this.fallbackLogger.LogError(ex.Message, new object[] { ex?.StackTrace!, ex?.InnerException!, ex?.Source! });
                if (ex is InvalidCredentialException authException)
                {
                    return new ObjectResult(authException.Message)
                    {
                        StatusCode = (int)HttpStatusCode.Unauthorized,
                        Value = authException.Message,
                    };
                }

                if (ex is CosmosException cosmosException)
                {
                    return this.objectResultProvider.HandleHttpError(cosmosException.StatusCode, cosmosException.Message);
                }

                if (ex is ExternalHttpClientException externalHttpClientException)
                {
                    return this.objectResultProvider.HandleHttpError(externalHttpClientException.StatusCode, externalHttpClientException.Message);
                }

                return this.objectResultProvider.ServerError(ex?.Message!);
            }

            this.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return new EmptyResult();
        }
    }
}
