
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace TicketDepot.TicketManagement.Repository
{
    /// <summary>
    /// Payment Info class.
    /// </summary>
    [JsonObject(
        Description = "Payment Info Document class.",
        ItemTypeNameHandling = TypeNameHandling.None,
        MissingMemberHandling = MissingMemberHandling.Ignore,
        ItemNullValueHandling = NullValueHandling.Ignore)]
    public class PaymentInfo
    {
        /// <summary>
        /// Initializes a new instance of <see cref="PaymentInfo"/>.
        /// </summary>
        /// <param name="customerAccountNumber"></param>
        /// <param name="transactionId"></param>
        /// <param name="totalPrice"></param>
        /// <param name="cardNumber"></param>
        /// <param name="expiration"></param>
        /// <param name="securityCode"></param>
        /// <param name="bankAccount"></param>
        /// <param name="bankRoutingNumber"></param>
        public PaymentInfo(string customerAccountNumber, string transactionId, decimal totalPrice, string? cardNumber = null, string? expiration = null, string? securityCode = null, string? bankAccount = null, string? bankRoutingNumber = null)
        {
            this.CustomerAccountNumber = customerAccountNumber;
            this.TransactionId = transactionId;
            this.TotalCharge = totalPrice;
            this.CardNumber = cardNumber;
            this.Expiration = expiration;
            this.SecurityCode = securityCode;
            this.BankAccount = bankAccount;
            this.BankRoutingNumber = bankRoutingNumber;
        }

        [Required]
        [JsonProperty("customerAccountNumber")]
        public string CustomerAccountNumber { get; set; }

        [JsonProperty("transactionId")]
        public string TransactionId { get; set; }

        [JsonProperty("totalCharge")]
        public decimal TotalCharge { get; set; }

        [JsonProperty("cardNumber")]
        public string? CardNumber { get; set; }

        [JsonProperty("expiration")]
        public string? Expiration { get; set; }

        [JsonProperty("securityCode")]
        public string? SecurityCode { get; set; }

        [JsonProperty("bankAccount")]
        public string? BankAccount { get; set; }

        [JsonProperty("bankRoutingNumber")]
        public string? BankRoutingNumber { get; set; }
    }
}
