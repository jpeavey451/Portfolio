
using Microsoft.Azure.Cosmos;

namespace TicketDepot.TicketManagement.Repository
{

    /// <summary>
    /// The class for the Reservation Container.
    /// </summary>
    public class ReservationContainer : IReservationContainer
    {
        /// <summary>
        /// Gets or sets the Event Container.
        /// </summary>
        public Container? Container { get; set; }
    }
}
