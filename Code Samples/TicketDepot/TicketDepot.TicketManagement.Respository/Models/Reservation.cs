using Newtonsoft.Json;

namespace TicketDepot.TicketManagement.Repository
{
    /// <summary>
    /// Class for reservations.
    /// </summary>
    [JsonObject(
        Description = "Reservation Document class.",
        ItemTypeNameHandling = TypeNameHandling.None,
        MissingMemberHandling = MissingMemberHandling.Ignore,
        ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Reservation : BaseReservation
    {
        /// <summary>
        /// Instantiates a new instance of <see cref="Reservation"/>.
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="ticketTypeId"></param>
        /// <param name="customerAccountNumber"></param>
        /// <param name="seatingType"></param>
        /// <param name="reserveMinutes"></param>
        /// <param name="quanity"></param>
        /// <param name="reservationStatus"></param>
        public Reservation(PaymentInfo paymentInfo, string transactionId, string eventId, string ticketTypeId, string customerAccountNumber, SeatingType seatingType, int reserveMinutes, int quanity, ReservationStatus reservationStatus)
            : base(transactionId, eventId, ticketTypeId, customerAccountNumber, seatingType, reserveMinutes, quanity, reservationStatus)
        {
            this.Id = Guid.NewGuid().ToString("N");
            if (reservationStatus == ReservationStatus.Reserved)
            {
                this.ReserveUntil = DateTimeOffset.UtcNow.AddMinutes(reserveMinutes);
            }

            this.CreatedDate = DateTimeOffset.UtcNow;
            this.PaymentInfo = paymentInfo;
        }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("_etag")]
        public string? Etag { get; set; }

        [JsonProperty("purchaseDate")]
        public DateTimeOffset? PurchaseDate { get; set; }

        [JsonProperty("emailSentId")]
        public string? EmailSentId { get; set; }

        [JsonProperty("sentByMailId")]
        public bool? SentByMailId { get; set; }

        [JsonProperty("createdDate")]
        public DateTimeOffset CreatedDate { get; set; }

        public PaymentInfo PaymentInfo { get; set; }

    }

}
