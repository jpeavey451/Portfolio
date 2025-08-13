
using Newtonsoft.Json;

namespace TicketDepot.TicketManagement.Repository
{
    /// <summary>
    /// Class for events
    /// </summary>
    [JsonObject(
        Description = "Event Document class.",
        ItemTypeNameHandling = TypeNameHandling.None,
        MissingMemberHandling = MissingMemberHandling.Ignore,
        ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Event : BaseEvent
    {
        /// <summary>
        /// Instantiates a new instance of <see cref="Event"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="venue"></param>
        /// <param name="description"></param>
        public Event(string name, string venueId, string description, EventType eventType, DateOnly eventDate, TimeOnly eventStartTime)
            : base(name, venueId, description, eventType, eventDate, eventStartTime)
        {
            this.Id = Guid.NewGuid().ToString("N");
            this.CreatedDate = DateTimeOffset.UtcNow;
        }

        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("_etag")]
        public string? Etag { get; set; }

        [JsonProperty("createdDate")]
        public DateTimeOffset CreatedDate { get; set; }
    }
}
