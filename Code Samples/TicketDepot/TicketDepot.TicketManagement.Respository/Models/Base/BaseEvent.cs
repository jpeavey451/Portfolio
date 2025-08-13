using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using TicketDepot.Shared;

namespace TicketDepot.TicketManagement.Repository
{
    /// <summary>
    /// The base Event class.
    /// </summary>
    public class BaseEvent
    {
        /// Instantiates a new instance of <see cref="BaseEvent"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="venue"></param>
        /// <param name="description"></param>
        public BaseEvent(string name, string venueId, string description, EventType eventType, DateOnly eventDate, TimeOnly eventStartTime)
        {
            this.Name = name;
            this.VenueId = venueId;
            this.Description = description;
            this.EventType = eventType;
            this.EventDate = eventDate;
            this.EventStartTime = eventStartTime;
        }

        [Required]
        [JsonProperty("venueId")]
        public string VenueId { get; set; }

        [JsonProperty("eventType")]
        public EventType EventType { get; set; }

        [Required]
        [JsonProperty("name")]
        public string Name { get; set; }

        [Required]
        [JsonProperty("description")]
        public string Description { get; set; }

        [FutureDate(ErrorMessage = "Event date must be in the future.")]
        [JsonProperty("eventDate")]
        public DateOnly EventDate { get; set; }

        /// <summary>
        /// 24 hour Clock times.
        /// </summary>
        [Required]
        [JsonProperty("eventStartTime")]
        public TimeOnly EventStartTime { get; set; }

        [Required]
        [JsonProperty("cancellationPeriodInMinutes")]
        public int CancellationPeriodInMinutes { get; set; }
    }
}
