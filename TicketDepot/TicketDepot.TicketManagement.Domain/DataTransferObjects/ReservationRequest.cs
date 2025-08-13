using TicketDepot.TicketManagement.Repository;

namespace TicketDepot.TicketManagement.Domain
{
    public class ReservationRequest : BaseReservation
    {
        /// <summary>
        /// Instantiates a new instance of <see cref="Reservation"/>.
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="eventId"></param>
        /// <param name="ticketTypeId"></param>
        /// <param name="customerAccountNumber"></param>
        /// <param name="seatingType"></param>
        /// <param name="reserveMinutes"></param>
        /// <param name="quanity"></param>
        /// <param name="reservationStatus"></param>
        public ReservationRequest(string transactionId, string eventId, string ticketTypeId, string customerAccountNumber, SeatingType seatingType, int reserveMinutes, int quanity, ReservationStatus reservationStatus)
            : base(transactionId, eventId, ticketTypeId, customerAccountNumber, seatingType, reserveMinutes, quanity, reservationStatus)
        {
            if (reservationStatus == ReservationStatus.Reserved)
            {
                this.ReserveUntil = DateTimeOffset.UtcNow.AddMinutes(reserveMinutes);
            }
        }
    }
}
