using Microsoft.Azure.Cosmos;

namespace TicketDepot.TicketManagement.Repository
{
    /// <summary>
    /// The interface for the instance of the <see cref="CosmosClient"/>.
    /// </summary>
    public interface ICosmosClient
    {
        /// <summary>
        /// Gets or sets the insance of the CosmosClient.
        /// </summary>
        CosmosClient? CosmosClientInstance { get; set; }

        /// <summary>
        /// Gets or sets the targeted database.
        /// </summary>
        Database? CosmosDatabase { get; set; }
    }
}
