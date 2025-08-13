
using Newtonsoft.Json;

namespace TicketDepot.Shared
{
    /// <summary>
    /// The Known Error class is to be used
    /// when we need to return both a Result 
    /// and a message.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class KnownError<T>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="KnownError"/>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="results"></param>
        public KnownError(string message, T results)
        {
            this.Message = message;
            this.Results = results;
        }

        /// <summary>
        /// Gets or sets the Message.
        /// Setting the serialization Order to ensure Message appears first.
        /// </summary>
        [JsonProperty(PropertyName = "message", Order = 0)]
        public string Message { get; set; }

        [JsonProperty(PropertyName = "results", Order = 1)]
        public T Results { get; set; }

    }
}
