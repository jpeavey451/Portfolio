using Microsoft.AspNetCore.Mvc;

namespace TicketDepot.TicketManagement.Repository
{
    public interface IAddExtension
    {
        Task<ObjectResult> AddAsync<T>(T item, CancellationToken cancellationToken = default)
            where T : class;
    }
}
