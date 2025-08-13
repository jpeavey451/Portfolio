using Microsoft.AspNetCore.Mvc;
using TicketDepot.TicketManagement.Repository;

namespace TicketDepot.TicketManagement.Domain
{
    /// <summary>
    /// Interface for <see cref="TicketTypeValidator"/>
    /// </summary>
    public interface ITicketTypeValidator
    {
        /// <summary>
        /// Validates a new <see cref="TicketType"/>.
        /// </summary>
        /// <param name="newTicketType"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ObjectResult> ValidateNewTicketType(TicketType newTicketType, CancellationToken cancellationToken = default);

        /// <summary>
        /// Validates an updated <see cref="TicketType"/>.
        /// </summary>
        /// <param name="updatedTicketType"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ObjectResult> ValidateUpdatedTicketType(TicketType updatedTicketType, CancellationToken cancellationToken = default);
    }
}