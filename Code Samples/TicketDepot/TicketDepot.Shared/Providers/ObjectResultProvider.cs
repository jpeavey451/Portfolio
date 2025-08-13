

namespace TicketDepot.Shared
{
    using System;
    using System.Globalization;
    using System.Net;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Formatters;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Helper class to assist in producing common responses.
    /// </summary>
    public class ObjectResultProvider : IObjectResultProvider
    {
        private readonly ILogger<ObjectResultProvider> logger;

        /// <summary>
        /// Creates an ObjectResult with a standardized external error message.
        /// </summary>
        /// <param name="statusCode">Status code returned from an external source.</param>
        /// <param name="message">The message to return.</param>
        /// <returns>ObjectResult for 500 server error with external error message.</returns>
        public ObjectResult HandleHttpError(HttpStatusCode statusCode, string message = "")
        {
            return new ObjectResult(string.Format(CultureInfo.InvariantCulture, HttpStatusCodeResponse.ExternalDependencyErrorCode, statusCode))
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Value = message,
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectResultProvider"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public ObjectResultProvider(ILogger<ObjectResultProvider> logger)
        {
            this.logger = logger;
        }

        /// <inheritdoc/>
        public ObjectResult Accepted(string message = HttpStatusCodeResponse.Accepted)
        {
            return new ObjectResult(message)
            {
                StatusCode = (int)HttpStatusCode.Accepted,
            };
        }

        /// <inheritdoc/>
        public ObjectResult Accepted<T>(T responseObject)
        {
            return new ObjectResult(responseObject)
            {
                StatusCode = (int)HttpStatusCode.Accepted,
            };
        }

        /// <inheritdoc/>
        public ObjectResult AlreadyReported(string message = HttpStatusCodeResponse.AlreadyReported)
        {
            return new ObjectResult(message)
            {
                StatusCode = (int)HttpStatusCode.AlreadyReported,
            };
        }

        /// <inheritdoc/>
        public ObjectResult AlreadyReported<T>(T responseObject)
        {
            return new ObjectResult(responseObject)
            {
                StatusCode = (int)HttpStatusCode.AlreadyReported,
            };
        }

        /// <inheritdoc/>
        public ObjectResult BadRequest(string message = HttpStatusCodeResponse.InvalidRequest)
        {
            return new ObjectResult(message)
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
            };
        }

        /// <inheritdoc/>
        public ObjectResult BadRequest<T>(T responseObject)
        {
            return new ObjectResult(responseObject)
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
            };
        }

        /// <inheritdoc/>
        public ObjectResult SetExceptionResult(string tagGuid, string message, Exception? ex = null, Uri? requestUri = null, params string[] values)
        {
            string logMessage = string.Format(CultureInfo.InvariantCulture, message, values);
            Exception newEx = new Exception($"{tagGuid}, {logMessage}, {ex?.Message} , {requestUri!}");
            this.logger.LogError(newEx, $"{tagGuid} {logMessage}");
            return this.ServerError(logMessage);
        }

        /// <inheritdoc/>
        public ObjectResult NoContent()
        {
            return new ObjectResult(HttpStatusCodeResponse.NoDataFound)
            {
                StatusCode = (int)HttpStatusCode.NoContent,
            };
        }

        /// <inheritdoc/>
        public ObjectResult NotModified(string message = HttpStatusCodeResponse.NotModified)
        {
            return new ObjectResult(message)
            {
                StatusCode = (int)HttpStatusCode.NotModified,
            };
        }

        /// <inheritdoc/>
        public ObjectResult NotModified<T>(T? responseObject = default)
        {
            return new ObjectResult(responseObject)
            {
                StatusCode = (int)HttpStatusCode.NotModified,
                ContentTypes = new MediaTypeCollection() { "application/json" },
            };
        }

        /// <inheritdoc/>
        public ObjectResult Ok(string message = HttpStatusCodeResponse.Success)
        {
            return new ObjectResult(message)
            {
                StatusCode = (int)HttpStatusCode.OK,
            };
        }

        /// <inheritdoc/>
        public ObjectResult Ok<T>(T? responseObject = default)
        {
            return new ObjectResult(responseObject)
            {
                StatusCode = (int)HttpStatusCode.OK,
                ContentTypes = new MediaTypeCollection() { "application/json" },
            };
        }

        /// <inheritdoc/>
        public ObjectResult PartialSuccess(string message = HttpStatusCodeResponse.PartialSuccess)
        {
            return new ObjectResult(message)
            {
                StatusCode = (int)HttpStatusCode.PartialContent,
            };
        }

        /// <inheritdoc/>
        public ObjectResult PreconditionFailed(string message = HttpStatusCodeResponse.PreconditionFailed)
        {
            return new ObjectResult(message)
            {
                StatusCode = (int)HttpStatusCode.PreconditionFailed,
            };
        }

        /// <inheritdoc/>
        public ObjectResult PreconditionFailed<T>(T? responseObject = null)
            where T : class
        {
            return new ObjectResult(responseObject)
            {
                StatusCode = (int)HttpStatusCode.PreconditionFailed,
                ContentTypes = new MediaTypeCollection() { "application/json" },
            };
        }

        /// <inheritdoc/>
        public ObjectResult RecordCreated(string message = HttpStatusCodeResponse.RecordCreated)
        {
            return new ObjectResult(message)
            {
                StatusCode = (int)HttpStatusCode.Created,
                ContentTypes = new MediaTypeCollection() { "application/json" },
            };
        }

        /// <inheritdoc/>
        public ObjectResult RecordCreated<T>(T? responseObject = null)
            where T : class
        {
            return new ObjectResult(responseObject)
            {
                StatusCode = (int)HttpStatusCode.Created,
                ContentTypes = new MediaTypeCollection() { "application/json" },
            };
        }

        /// <inheritdoc/>
        public ObjectResult RecordCreatedWithFurtherAction<T>(T? responseObject = null)
            where T : class
        {
            return new ObjectResult(responseObject)
            {
                StatusCode = (int)HttpStatusCode.Accepted,
                ContentTypes = new MediaTypeCollection() { "application/json" },
            };
        }

        /// <inheritdoc/>
        public ObjectResult RecordDeletedWithFurtherAction<T>(T? responseObject = null)
            where T : class
        {
            return new ObjectResult(responseObject)
            {
                StatusCode = (int)HttpStatusCode.Accepted,
                ContentTypes = new MediaTypeCollection() { "application/json" },
            };
        }

        /// <inheritdoc/>
        public ObjectResult RecordExists(string message = HttpStatusCodeResponse.RecordExists)
        {
            return new ObjectResult(message)
            {
                StatusCode = (int)HttpStatusCode.Conflict,
            };
        }

        /// <inheritdoc/>
        public ObjectResult RecordExists<T>(T responseObject)
        {
            return new ObjectResult(responseObject)
            {
                StatusCode = (int)HttpStatusCode.Conflict,
                ContentTypes = new MediaTypeCollection() { "application/json" },
            };
        }

        /// <inheritdoc/>
        public ObjectResult ResourceNotFound(string message = HttpStatusCodeResponse.NoDataFound)
        {
            return new ObjectResult(message)
            {
                StatusCode = (int)HttpStatusCode.NotFound,
            };
        }

        /// <inheritdoc/>
        public ObjectResult ResourceNotFound<T>(T responseObject)
        {
            return new ObjectResult(responseObject)
            {
                StatusCode = (int)HttpStatusCode.NotFound,
                ContentTypes = new MediaTypeCollection() { "application/json" },
            };
        }

        /// <inheritdoc/>
        public ObjectResult UnAuthorized(string message = HttpStatusCodeResponse.UnAuthorized)
        {
            return new ObjectResult(message)
            {
                StatusCode = (int)HttpStatusCode.Unauthorized,
            };
        }

        /// <inheritdoc/>
        public ObjectResult UnAuthorized<T>(T? responseObject = null)
            where T : class
        {
            return new ObjectResult(responseObject)
            {
                StatusCode = (int)HttpStatusCode.Unauthorized,
                ContentTypes = new MediaTypeCollection() { "application/json" },
            };
        }

        /// <inheritdoc/>
        public ObjectResult Forbidden(string message = HttpStatusCodeResponse.Forbidden)
        {
            return new ObjectResult(message)
            {
                StatusCode = (int)HttpStatusCode.Forbidden,
            };
        }

        /// <inheritdoc/>
        public ObjectResult Forbidden<T>(T? responseObject = null)
            where T : class
        {
            return new ObjectResult(responseObject)
            {
                StatusCode = (int)HttpStatusCode.Forbidden,
                ContentTypes = new MediaTypeCollection() { "application/json" },
            };
        }

        /// <inheritdoc/>
        public ObjectResult ApiException(ApiException apiException)
        {
            return new ObjectResult(apiException)
            {
                StatusCode = apiException?.StatusCode,
            };
        }

        /// <inheritdoc/>
        public ObjectResult Found()
        {
            return new ObjectResult(HttpStatusCodeResponse.Found)
            {
                StatusCode = (int)HttpStatusCode.Found,
            };
        }

        /// <inheritdoc/>
        public ObjectResult Found<T>(T responseObject)
        {
            return new ObjectResult(responseObject)
            {
                StatusCode = (int)HttpStatusCode.Found,
            };
        }

        /// <inheritdoc/>
        public ObjectResult TimeOut(string message = HttpStatusCodeResponse.TimeOut)
        {
            return new ObjectResult(message)
            {
                StatusCode = (int)HttpStatusCode.NotFound,
            };
        }

        /// <inheritdoc/>
        public ObjectResult TimeOut<T>(T responseObject)
        {
            return new ObjectResult(responseObject)
            {
                StatusCode = (int)HttpStatusCode.RequestTimeout,
            };
        }

        /// <inheritdoc/>
        public ObjectResult ServerError(string message = HttpStatusCodeResponse.InternalServerError)
        {
            return new ObjectResult(message)
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
            };
        }

        /// <inheritdoc/>
        public ObjectResult ServerError<T>(T responseObject)
        {
            return new ObjectResult(responseObject)
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
            };
        }
    }
}
