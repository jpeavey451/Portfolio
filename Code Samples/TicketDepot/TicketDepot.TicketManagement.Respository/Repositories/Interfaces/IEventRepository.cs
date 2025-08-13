
using Microsoft.AspNetCore.Mvc;

namespace TicketDepot.TicketManagement.Repository
{
    /// <summary>
    /// Interface for <see cref="EventRepository"/>.
    /// </summary>
    public interface IEventRepository : IRepository, INameExtension, IGetAllExtension, IUpdateExtension, IAddExtension
    {
        /// <summary>
        /// Gets Events for the specified Venue on the specified date.
        /// </summary>
        /// <param name="venueId"></param>
        /// <param name="eventDate"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ObjectResult> GetEventsByVenueIdAndDate(string venueId, DateOnly eventDate, CancellationToken cancellationToken = default);
    }
}
