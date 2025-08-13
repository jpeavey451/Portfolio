
using Microsoft.AspNetCore.Mvc;

namespace TicketDepot.TicketManagement.Repository
{
    /// <summary>
    /// Interface for <see cref="TicketTypeRepository"/>.
    /// </summary>
    public interface ITicketTypeRepository : IRepository, IUpdateExtension, IAddExtension
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
    }
}
