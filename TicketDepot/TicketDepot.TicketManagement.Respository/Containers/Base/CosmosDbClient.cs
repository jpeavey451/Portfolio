
namespace TicketDepot.TicketManagement.Repository
{
    using Microsoft.Azure.Cosmos;

    /// <inheritdoc />
    public class CosmosDbClient : ICosmosClient
    {
        /// <inheritdoc />
        public CosmosClient? CosmosClientInstance { get; set; }

        /// <inheritdoc />
        public Database? CosmosDatabase { get; set; }
    }
}
