
using Microsoft.Azure.Cosmos;

namespace TicketDepot.TicketManagement.Repository
{

    /// <summary>
    /// The class for the Customer Container.
    /// </summary>
    public class CustomerContainer : ICustomerContainer
    {
        /// <summary>
        /// Gets or sets the Event Container.
        /// </summary>
        public Container? Container { get; set; }
    }
}
