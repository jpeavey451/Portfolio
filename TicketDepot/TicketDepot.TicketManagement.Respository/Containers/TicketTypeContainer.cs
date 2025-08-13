
using Microsoft.Azure.Cosmos;

namespace TicketDepot.TicketManagement.Repository
{

    /// <summary>
    /// The class for the Tickets Container.
    /// </summary>
    public class TicketTypeContainer : ITicketTypeContainer
    {
        /// <summary>
        /// Gets or sets the Event Container.
        /// </summary>
        public Container? Container { get; set; }
    }
}
