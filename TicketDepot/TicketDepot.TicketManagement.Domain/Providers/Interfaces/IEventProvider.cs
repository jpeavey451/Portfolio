
using Microsoft.AspNetCore.Mvc;
using TicketDepot.TicketManagement.Repository;

namespace TicketDepot.TicketManagement.Domain
{
    /// <summary>
    /// Interface for <see cref="EventProvider"/>.
    /// </summary>
    public interface IEventProvider
    {
        /// <summary>
        /// Creates a new <see cref="Event"/>. Events are unique by name.
        /// </summary>
        /// <param name="newEvent">The <see cref="Event"/> to create a new <see cref="Event"/> from.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The <see cref="ObjectResult"/> indicating success or failure. If success, containing the new <see cref="Event"/></returns>
        Task<ObjectResult> CreateEventAsync(Event newEvent, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing <see cref="Event"/>. Events are unique by name.
        /// </summary>
        /// <param name="updatedEvent">The <see cref="Event"/> to update.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The <see cref="ObjectResult"/> indicating success or failure. If success, containing the updated <see cref="Event"/></returns>
        Task<ObjectResult> UpdateEventAsync(Event updatedEvent, CancellationToken cancellationToken = default);

        /// <summary>
        /// method to get the event information by name.
        /// </summary>
        /// <param name="eventName">Name of the Event.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The <see cref="ObjectResult"/> containing the <see cref="Event"/> or a HttpStatusCode indicating why not.</returns>
        Task<ObjectResult> GetEventByNameAsync(string eventName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the availalbe seats by event id.
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>The <see cref="ObjectResult"/> list of ticket types and number of available seats.</returns>
        Task<ObjectResult> GetAvailabilityAsync(string eventId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Reserves Tickets for a specfied time period.
        /// </summary>
        /// <param name="reservation"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ObjectResult> ReserveTicketsAsync(Reservation reservation, CancellationToken cancellationToken = default);

        /// <summary>
        /// Purchases Tickets.
        /// </summary>
        /// <param name="reservation"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ObjectResult> PurchaseTicketsAsync(Reservation reservation, CancellationToken cancellationToken = default);

        /// <summary>
        /// Cancels Tickets.
        /// </summary>
        /// <param name="reservationId"></param>
        /// <param name="transactionId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ObjectResult> CancelTicketsAsync(string reservationId, string transactionId, CancellationToken cancellationToken = default);
    }
}
