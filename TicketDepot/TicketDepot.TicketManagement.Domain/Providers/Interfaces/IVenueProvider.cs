
using Microsoft.AspNetCore.Mvc;
using TicketDepot.TicketManagement.Repository;

namespace TicketDepot.TicketManagement.Domain
{
    public interface IVenueProvider
    {
        /// <summary>
        /// Creates a new <see cref="Venue"/>. Venues are unique by name.
        /// </summary>
        /// <param name="newVenue">The <see cref="Venue"/> to create a new <see cref="Venue"/> from.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The <see cref="ObjectResult"/> indicating success or failure. If success, containing the new <see cref="Venue"/></returns>
        Task<ObjectResult> CreateVenueAsync(Venue newVenue, CancellationToken cancellationToken);

        /// <summary>
        /// Creates a new <see cref="Venue"/>. Venues are unique by name.
        /// </summary>
        /// <param name="updatedVenue">The <see cref="Venue"/> to update.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The <see cref="ObjectResult"/> indicating success or failure. If success, containing the updated <see cref="Venue"/></returns>
        Task<ObjectResult> UpdateVenueAsync(Venue updatedVenue, CancellationToken cancellationToken);

        /// <summary>
        /// method to get the venu information by venue name.
        /// </summary>
        /// <param name="venueName">Name of the Venue.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The <see cref="ObjectResult"/> containing the <see cref="Venue"/> or a HttpStatusCode indicating why not.</returns>
        Task<ObjectResult> GetVenueByNameAsync(string venueName, CancellationToken cancellationToken);

        /// <summary>
        /// method to get the venu information by venue name.
        /// </summary>
        /// <param name="venueId">Venue Id.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The <see cref="ObjectResult"/> containing the <see cref="Venue"/> or a HttpStatusCode indicating why not.</returns>
        Task<ObjectResult> GetVenueByVenueIdAsync(string venueId, CancellationToken cancellationToken);

        /// <summary>
        /// Gets all Venues paginated.
        /// </summary>
        /// <param name="continuationToken"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>The <see cref="ObjectResult"/> containing the <see cref="Venue"/> or a HttpStatusCode indicating why not.</returns>
        Task<ObjectResult> GetAllVenues(string continuationToken, CancellationToken cancellationToken);
    }
}
