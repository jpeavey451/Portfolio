using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace TicketDepot.TicketManagement.Repository
{
    /// <summary>
    /// The transaction class.
    /// </summary>
    public class Transaction
    {
        /// <summary>
        /// Instantiates a new instance of <see cref="Transaction"/>.
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="reservationId"></param>
        public Transaction(string transactionId, string reservationId)
        {
            this.TransactionId = transactionId;
            this.ReservationId = reservationId;
        }

        [Required]
        [JsonProperty("transactionId")]
        public string TransactionId { get; set; }

        [Required]
        [JsonProperty("reservationId")]
        public string ReservationId { get; set; }

        [JsonProperty("isSuccess")]
        public bool IsSuccess { get; set; } = false;

    }
}
