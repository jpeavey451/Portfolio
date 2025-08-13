using System.Text.Json.Serialization;

namespace TicketDepot.TicketManagement.Repository
{
    /// <summary>
    /// SeatingType enum.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SeatingType
    {
        Unknown = 0,
        GeneralAdmission,
        ReservedSeating,
        VIP,
    }
}
