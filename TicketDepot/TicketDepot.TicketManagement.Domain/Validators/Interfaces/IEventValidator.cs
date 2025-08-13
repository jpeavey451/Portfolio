using Microsoft.AspNetCore.Mvc;
using TicketDepot.TicketManagement.Repository;

namespace TicketDepot.TicketManagement.Domain
{
    /// <summary>
    /// Validator for <see cref="Event"/>.
    /// </summary>
    public interface IEventValidator
    {
        /// <summary>
        /// Validates a new <see cref="Event"/>.
        /// </summary>
        /// <param name="newEvent"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ObjectResult> ValidateNewEvent(Event newEvent, CancellationToken cancellationToken = default);

        /// <summary>
        /// Validates an updated <see cref="Event"/>.
        /// </summary>
        /// <param name="updatedEvent"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ObjectResult> ValidateUpdatedEvent(Event updatedEvent, CancellationToken cancellationToken = default);

        /// <summary>
        /// Validates a <see cref="Reservation"/> for reservation.
        /// </summary>
        /// <param name="reservation"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ObjectResult> ValidateReserveTicket(Reservation reservation, CancellationToken cancellationToken = default);

        /// <summary>
        /// Validates a <see cref="Reservation"/> for purchase.
        /// </summary>
        /// <param name="reservation"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ObjectResult> ValidatePurchaseTicketsAsync(Reservation reservation, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets availailibility by EventId, Reservation status and Seating Type.
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ObjectResult> ValidateGetAvailability(string eventId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Cancels a <see cref="Reservation"/>.
        /// </summary>
        /// <param name="reservationId"></param>
        /// <param name="transactionId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ObjectResult> CancelTicketsValidation(string reservationId, string transactionId, CancellationToken cancellationToken = default);
    }
}
