using Microsoft.Azure.Cosmos;

namespace TicketDepot.TicketManagement.Repository
{

    /// <summary>
    /// The interface for the container instance.
    /// </summary>
    public interface ICosmosContainer
    {
        /// <summary>
        /// Gets or sets the specific container.
        /// </summary>
        Container? Container { get; set; }
    }
}
