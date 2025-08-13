
using Microsoft.Azure.Cosmos;

namespace TicketDepot.TicketManagement.Repository
{

    /// <summary>
    /// The class for the Products Container.
    /// </summary>
    public class TransactionContainer : ITransactionContainer
    {
        /// <summary>
        /// Gets or sets the Event Container.
        /// </summary>
        public Container? Container { get; set; }
    }
}
