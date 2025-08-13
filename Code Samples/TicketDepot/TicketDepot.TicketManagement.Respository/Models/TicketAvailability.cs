using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace TicketDepot.TicketManagement.Repository
{
    /// <summary>
    /// The TicketAvailablity class.
    /// </summary>
    [JsonObject(
        Description = "Reservation Document class.",
        ItemTypeNameHandling = TypeNameHandling.None,
        MissingMemberHandling = MissingMemberHandling.Ignore,
        ItemNullValueHandling = NullValueHandling.Ignore)]
    public class TicketAvailability
    {
        [Required]
        [JsonProperty("reservationStatus")]
        public ReservationStatus ReservationStatus { get; set; }

        [JsonProperty("seatingType")]
        public SeatingType SeatingType { get; set; }

        [JsonProperty("countOf")]
        public int CountOf { get; set; }
    }
}
