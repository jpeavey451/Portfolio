
namespace TicketDepot.Shared
{
    using System;
    using System.Net;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Methods for results.
    /// </summary>
    public interface IObjectResultProvider
    {
        /// <summary>
        /// Creates an ObjectResult with a standardized external error message.
        /// </summary>
        /// <param name="statusCode">Status code returned from an external source.</param>
        /// <param name="message">The message to return.</param>
        /// <returns>ObjectResult for 500 server error with external error message.</returns>
        ObjectResult HandleHttpError(HttpStatusCode statusCode, string message = "");

        /// <summary>
        /// Create a standard server 200 response.
        /// </summary>
        /// <param name="message">The message to return.</param>
        /// <returns>An ObjectResult for 200 server response.</returns>
        ObjectResult Ok(string message = HttpStatusCodeResponse.Success);

        /// <summary>
        /// Create an ObjectResult response for a successful processing.
        /// </summary>
        /// <typeparam name="T">The type of the response object.</typeparam>
        /// <param name="responseObject">The object for the response.</param>
        /// <returns>An ObjectResult of 200 with a standard message.</returns>
        ObjectResult Ok<T>(T? responseObject = default);

        /// <summary>
        /// Create an ObjectResult response for a record created.
        /// </summary>
        /// <param name="message">The message for the response.</param>
        /// <returns>An ObjectResult of 201 with a response object.</returns>
        ObjectResult RecordCreated(string message = HttpStatusCodeResponse.RecordCreated);

        /// <summary>
        /// Create an ObjectResult response for a record created.
        /// </summary>
        /// <typeparam name="T">The type of the response object.</typeparam>
        /// <param name="responseObject">The object for the response.</param>
        /// <returns>An ObjectResult of 201 with a response object.</returns>
        ObjectResult RecordCreated<T>(T? responseObject = null)
            where T : class;

        /// <summary>
        /// Create an ObjectResult response for a record created but with further activity.
        /// </summary>
        /// <typeparam name="T">The type of the response object.</typeparam>
        /// <param name="responseObject">The object for the response.</param>
        /// <returns>An ObjectResult of 202 with a response object.</returns>
        ObjectResult RecordCreatedWithFurtherAction<T>(T? responseObject = null)
            where T : class;

        /// <summary>
        /// Create an ObjectResult response for a record created.
        /// </summary>
        /// <typeparam name="T">The type of the response object.</typeparam>
        /// <param name="responseObject">The object for the response.</param>
        /// <returns>An ObjectResult of 201 with a response object.</returns>
        ObjectResult RecordDeletedWithFurtherAction<T>(T? responseObject = null)
            where T : class;

        /// <summary>
        /// Creates an ObjectResult with a 202 status.
        /// </summary>
        /// <param name="message">Message to send with error.</param>
        /// <returns>An ObjectResult.</returns>
        ObjectResult Accepted(string message = HttpStatusCodeResponse.Accepted);

        /// <summary>
        /// Creates an ObjectResult with a 202 status.
        /// </summary>
        /// <typeparam name="T">The type of the response object.</typeparam>
        /// <param name="responseObject">The object for the response.</param>
        /// <returns>An ObjectResult.</returns>
        ObjectResult Accepted<T>(T responseObject);

        /// <summary>
        /// Create an ObjectResult response for no content.
        /// </summary>
        /// <returns>An ObjectResult of 204 with a standard message.</returns>
        ObjectResult NoContent();

        /// <summary>
        /// Create an ObjectResult response for a record already exists conflict error.
        /// </summary>
        /// <param name="message">The message to return.</param>
        /// <returns>An ObjectResult of 208 with a standard message.</returns>
        ObjectResult AlreadyReported(string message = HttpStatusCodeResponse.AlreadyReported);

        /// <summary>
        /// Create an ObjectResult response for a record already exists conflict error.
        /// </summary>
        /// <typeparam name="T">The type of the response object.</typeparam>
        /// <param name="responseObject">The object for the response.</param>
        /// <returns>An ObjectResult of 208 with a result object.</returns>
        ObjectResult AlreadyReported<T>(T responseObject);

        /// <summary>
        /// Create an ObjectResult response for record(s) found.
        /// </summary>
        /// <returns>An ObjectResult of 302 with a standard message.</returns>
        ObjectResult Found();

        /// <summary>
        /// Create an ObjectResult response for record(s) found.
        /// </summary>
        /// <typeparam name="T">The type of the response object.</typeparam>
        /// <param name="responseObject">The object for the response.</param>
        /// <returns>An ObjectResult of 302 with a standard message.</returns>
        ObjectResult Found<T>(T responseObject);

        /// <summary>
        /// Create an ObjectResult response for a 'NotModified' result.
        /// </summary>
        /// <typeparam name="T">The type of the response object.</typeparam>
        /// <param name="responseObject">The object for the response.</param>
        /// <returns>An ObjectResult of 304 with an object.</returns>
        ObjectResult NotModified<T>(T? responseObject = default);

        /// <summary>
        /// Create an ObjectResult response for a 'NotModified' result.
        /// </summary>
        /// <param name="message">The message to return.</param>
        /// <returns>An ObjectResult for 304 server response.</returns>
        ObjectResult NotModified(string message = HttpStatusCodeResponse.NotModified);

        /// <summary>
        /// Creates an ObjectResult with a 400 bad request error.
        /// </summary>
        /// <param name="message">Message to send with error.</param>
        /// <returns>An ObjectResult.</returns>
        ObjectResult BadRequest(string message = HttpStatusCodeResponse.InvalidRequest);

        /// <summary>
        /// Creates an ObjectResult with a 400 bad request error.
        /// </summary>
        /// <typeparam name="T">The type of the response object.</typeparam>
        /// <param name="responseObject">The object for the response.</param>
        /// <returns>An ObjectResult.</returns>
        ObjectResult BadRequest<T>(T responseObject);

        /// <summary>
        /// Creates an ObjectResult with a 400 bad request error.
        /// </summary>
        /// <param name="message">Message to send with error.</param>
        /// <returns>An ObjectResult.</returns>
        ObjectResult PartialSuccess(string message = HttpStatusCodeResponse.PartialSuccess);

        /// <summary>
        /// Creates an ObjectResult with a 400 bad request error.
        /// </summary>
        /// <param name="message">Message to send with error.</param>
        /// <returns>An ObjectResult.</returns>
        ObjectResult PreconditionFailed(string message = HttpStatusCodeResponse.PreconditionFailed);

        /// <summary>
        /// Creates an ObjectResult with a 400 bad request error.
        /// </summary>
        /// <typeparam name="T">The type of the response object.</typeparam>
        /// <param name="responseObject">The object for the response.</param>
        /// <returns>An ObjectResult.</returns>
        ObjectResult PreconditionFailed<T>(T? responseObject = null)
            where T : class;

        /// <summary>
        /// Creates an ObjectResult with a 400 bad request error.
        /// </summary>
        /// <param name="message">Message to send with error.</param>
        /// <returns>An ObjectResult.</returns>
        ObjectResult TimeOut(string message = HttpStatusCodeResponse.TimeOut);

        /// <summary>
        /// Creates an ObjectResult with a 400 bad request error.
        /// </summary>
        /// <typeparam name="T">The type of the response object.</typeparam>
        /// <param name="responseObject">The object for the response.</param>
        /// <returns>An ObjectResult.</returns>
        ObjectResult TimeOut<T>(T responseObject);

        /// <summary>
        /// Create an ObjectResult response for a un-authorized action.
        /// </summary>
        /// <param name="message">The object for the response.</param>
        /// <returns>An ObjectResult of 401 with a response object.</returns>
        ObjectResult UnAuthorized(string message = HttpStatusCodeResponse.UnAuthorized);

        /// <summary>
        /// Create an ObjectResult response for a un-authorized action.
        /// </summary>
        /// <typeparam name="T">The type of the response object.</typeparam>
        /// <param name="responseObject">The object for the response.</param>
        /// <returns>An ObjectResult of 401 with a response object.</returns>
        ObjectResult UnAuthorized<T>(T? responseObject = null)
            where T : class;

        /// <summary>
        /// Create an ObjectResult response for an action without sufficient permission.
        /// </summary>
        /// <param name="message">The object for the response.</param>
        /// <returns>An ObjectResult of 403 with a response object.</returns>
        ObjectResult Forbidden(string message = HttpStatusCodeResponse.Forbidden);

        /// <summary>
        /// Create an ObjectResult response for an action without sufficient permission.
        /// </summary>
        /// <typeparam name="T">The type of the response object.</typeparam>
        /// <param name="responseObject">The object for the response.</param>
        /// <returns>An ObjectResult of 403 with a response object.</returns>
        ObjectResult Forbidden<T>(T? responseObject = null)
            where T : class;

        /// <summary>
        /// Create an ObjectResult response for ApiException thrown by auto-generated client.
        /// </summary>
        /// <param name="apiException">The object for the response.</param>
        /// <returns>An ObjectResult of 401 with a response object.</returns>
        public ObjectResult ApiException(ApiException apiException);

        /// <summary>
        /// Creates a 404 response object with a message. Should be used for REST missing resource responses, not search query API endpoints.
        /// </summary>
        /// <param name="message">Message to include with the response. Default to ERROR_MESSAGE_NO_DATA_FOUND.</param>
        /// <returns>A ObjectResult response for a 404 error.</returns>
        ObjectResult ResourceNotFound(string message = HttpStatusCodeResponse.NoDataFound);

        /// <summary>
        /// Creates a 404 response object with a message. Should be used for REST missing resource responses, not search query API endpoints.
        /// </summary>
        /// <typeparam name="T">The type of the response object.</typeparam>
        /// <param name="responseObject">The object for the response.</param>
        /// <returns>A ObjectResult response for a 404 error.</returns>
        ObjectResult ResourceNotFound<T>(T responseObject);

        /// <summary>
        /// Create an ObjectResult response for a record already exists conflict error.
        /// </summary>
        /// <param name="message">The message to return.</param>
        /// <returns>An ObjectResult of 409 with a standard message.</returns>
        ObjectResult RecordExists(string message = HttpStatusCodeResponse.RecordExists);

        /// <summary>
        /// Create an ObjectResult response for a record already exists conflict error.
        /// </summary>
        /// <typeparam name="T">The type of the response object.</typeparam>
        /// <param name="responseObject">The object for the response.</param>
        /// <returns>An ObjectResult of 409 with a standard message.</returns>
        ObjectResult RecordExists<T>(T responseObject);

        /// <summary>
        /// Create a standard server 500 error.
        /// </summary>
        /// <param name="message">The message to return.</param>
        /// <returns>An ObjectResult for 500 server error.</returns>
        ObjectResult ServerError(string message = HttpStatusCodeResponse.InternalServerError);

        /// <summary>
        /// Create a standard server 500 error.
        /// </summary>
        /// <typeparam name="T">The type of the response object.</typeparam>
        /// <param name="responseObject">The object for the response.</param>
        /// <returns>A ObjectResult response for a 404 error.</returns>
        ObjectResult ServerError<T>(T responseObject);

        /// <summary>
        /// Logs the exception and returns a server error.
        /// </summary>
        /// <param name="tagGuid">The guid tag.</param>
        /// <param name="message">The message.</param>
        /// <param name="ex">The <see cref="Exception"/>.</param>
        /// <param name="requestUri">The Request URI.</param>
        /// <param name="values">Optional values to log.</param>
        /// <returns>The <see cref="ObjectResult"/>.</returns>
        ObjectResult SetExceptionResult(string tagGuid, string message, Exception? ex = null, Uri? requestUri = null, params string[] values);
    }
}