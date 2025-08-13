using System.Text.Json.Serialization;

namespace TicketDepot.TicketManagement.Repository.Enums
{
    /// <summary>
    /// Payment type enum.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PaymentType
    {
        Unknown = 0,
        CreditCard, 
        BankAccount,
    }
}
