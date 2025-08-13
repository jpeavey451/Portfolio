using System.Globalization;

namespace TicketDepo.TicketManagement.Clients
{
    /// <summary>
    /// Interface for <see cref="ClientProvider"/>.
    /// </summary>
    public interface IClientProvider
    {
        /// <summary>
        /// Converts an object to a string.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="cultureInfo"></param>
        /// <returns></returns>
        string ConvertToString(object value, CultureInfo cultureInfo);

        /// <summary>
        /// Reads a <see cref="HttpResponseMessage"/> and converts it into an <see cref="ObjectResponseResult{T}"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response"></param>
        /// <param name="headers"></param>
        /// <param name="ReadResponseAsString"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ObjectResponseResult<T>> ReadObjectResponseAsync<T>(HttpResponseMessage response, IReadOnlyDictionary<string, IEnumerable<string>> headers, bool ReadResponseAsString, CancellationToken cancellationToken);
    }
}