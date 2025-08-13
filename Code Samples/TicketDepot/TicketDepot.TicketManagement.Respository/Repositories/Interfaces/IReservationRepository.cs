
using Microsoft.AspNetCore.Mvc;
using TicketDepot.TicketManagement.Repository;

namespace TicketDepot.TicketManagement
{
    /// <summary>
    /// Interface for class <see cref="ReservationRepository"/>.
    /// </summary>
    public interface IReservationRepository : IRepository, IUpdateExtension, IAddExtension
    {
        /// <summary>
        /// Gets the entire collection, paginated, by event id.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="continuationToken"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ObjectResult> GetAllByEventIdAsync<T>(string eventId, string? continuationToken = null, CancellationToken cancellationToken = default)
            where T : class;

        /// <summary>
        /// Gets the count of tickets by <see cref="ReservationStatus"/> and <see cref="SeatingType"/>.
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ObjectResult> GetCountOfTicketsReservedAsync(string eventId, CancellationToken cancellationToken = default);
    }
}
