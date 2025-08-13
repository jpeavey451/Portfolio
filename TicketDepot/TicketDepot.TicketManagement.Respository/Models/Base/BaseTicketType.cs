using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace TicketDepot.TicketManagement.Repository
{
    public class BaseTicketType
    {
        /// <summary>
        /// Initializes a new instance of <see cref="BaseTicketType"/>.
        /// </summary>
        /// <param name="seatingType"></param>
        /// <param name="price"></param>
        /// <param name="quantity"></param>
        /// <param name="eventId"></param>
        public BaseTicketType(string venueId, SeatingType seatingType, decimal price, string eventId)
        {
            this.SeatingType = seatingType;
            this.Price = price;
            this.EventId = eventId;
            this.VenueId = venueId;
        }

        [Range(0, 60)]
        public int ReserveMinutes { get; set; }

        [Required]
        [JsonProperty("venueId")]
        public string VenueId { get; set; }

        [Required]
        [JsonProperty("eventId")]
        public string EventId { get; set; }

        [Required]
        [JsonProperty("seatingType")]
        public SeatingType SeatingType { get; set; } // e.g., General, VIP

        [Required]
        [JsonProperty("price")]
        public decimal Price { get; set; }
    }
}
