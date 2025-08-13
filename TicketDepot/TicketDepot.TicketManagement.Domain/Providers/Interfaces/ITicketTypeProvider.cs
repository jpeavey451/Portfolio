
using Microsoft.AspNetCore.Mvc;
using TicketDepot.TicketManagement.Repository;

namespace TicketDepot.TicketManagement.Domain
{
    public interface ITicketTypeProvider
    {
        /// <summary>
        /// Creates a new <see cref="TicketType"/>. TicketTypes are unique by EventId and <see cref="SeatingType"/>.
        /// </summary>
        /// <param name="newTicketType">The <see cref="TicketType"/> to create a new <see cref="TicketType"/> from.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The <see cref="ObjectResult"/> indicating success or failure. If success, containing the new <see cref="TicketType"/></returns>
        Task<ObjectResult> CreateTicketTypeAsync(TicketType newTicketType, CancellationToken cancellationToken);

        /// <summary>
        /// Updates an existing <see cref="TicketType"/>. TicketTypes are unique by <see cref="BaseTicketType.EventId"/> and <see cref="SeatingType"/>.
        /// </summary>
        /// <param name="updatedTicketType">The <see cref="TicketType"/> to Update.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The <see cref="ObjectResult"/> indicating success or failure. If success, containing the new <see cref="TicketType"/></returns>
        Task<ObjectResult> UpdateTicketTypeAsync(TicketType updatedTicketType, CancellationToken cancellationToken);
    }
}
