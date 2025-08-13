using System.Text.Json.Serialization;

namespace TicketDepot.TicketManagement.Repository
{
    /// <summary>
    /// Reservation Status enum.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ReservationStatus
    {
        Unknown = 0,
        Reserved,
        Purchased,
        Cancelled,
    }
}
