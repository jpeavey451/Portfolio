using Microsoft.AspNetCore.Mvc;
using TicketDepot.TicketManagement.Repository;

namespace TicketDepot.TicketManagement.Domain
{
    /// <summary>
    /// Interface of <see cref="VenueValidator"/>.
    /// </summary>
    public interface IVenueValidator
    {
        /// <summary>
        /// Validates a new <see cref="Venue"/>.
        /// </summary>
        /// <param name="newVenue"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ObjectResult> ValidateNewVenu(Venue newVenue, CancellationToken cancellationToken = default);

        /// <summary>
        /// Validates an updated Venue.
        /// </summary>
        /// <param name="updatedVenue"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ObjectResult> ValidateUpdateVenue(Venue updatedVenue, CancellationToken cancellationToken = default);
    }
}