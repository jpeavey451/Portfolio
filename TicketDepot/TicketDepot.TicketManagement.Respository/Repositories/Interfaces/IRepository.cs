
using Microsoft.AspNetCore.Mvc;

namespace TicketDepot.TicketManagement
{
    /// <summary>
    /// Base interface for repositories.
    /// </summary>
    public interface IRepository
    {
        Task<ObjectResult> GetByIdAsync<T>(string id, CancellationToken cancellationToken = default)
            where T : class;
    }
}