using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace TicketDepot.TicketManagement.Repository
{
    /// <summary>
    /// The TicketType class.
    /// </summary>
    [JsonObject(
        Description = "TicketType Document class.",
        ItemTypeNameHandling = TypeNameHandling.None,
        MissingMemberHandling = MissingMemberHandling.Ignore,
        ItemNullValueHandling = NullValueHandling.Ignore)]
    public class TicketType : BaseTicketType
    {
        /// <summary>
        /// Initializes a new instance of <see cref="TicketType"/>.
        /// </summary>
        /// <param name="seatingType"></param>
        /// <param name="price"></param>
        /// <param name="quantity"></param>
        /// <param name="eventId"></param>
        public TicketType(string venueId, SeatingType seatingType, decimal price, int capacity, string eventId)
            : base(venueId, seatingType, price, eventId)
        {
            this.Id = Guid.NewGuid().ToString("N");
            this.Capacity = capacity;
            this.CreatedDate = DateTimeOffset.UtcNow;
        }

        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("_etag")]
        public string? Etag { get; set; }

        [JsonProperty("Capacity")]
        [Range(0,100000)]
        public int Capacity { get; set; }

        [JsonProperty("createdDate")]
        public DateTimeOffset CreatedDate { get; set; }
    }

}
