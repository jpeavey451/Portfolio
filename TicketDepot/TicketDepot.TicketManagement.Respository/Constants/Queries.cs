
namespace TicketDepot.TicketManagement.Repository
{
    /// <summary>
    /// Cosmos Queries class.
    /// </summary>
    internal static class Queries
    {
        internal const string GetByName = "SELECT TOP * FROM c WHERE StringEquals(c.name, @name, true)";
        internal const string GetById = "SELECT * FROM c WHERE c.id = @id";
        internal const string GetByTransactionId = "SELECT * FROM c WHERE c.TransactionId = @transactionId";
        internal const string GetAllItems = "SELECT * FROM c ORDER BY c._ts DESC";
        internal const string GetEventByVenueIdAndDate = "SELECT * FROM c WHERE c.VenueId = @venueId AND c.eventDate = @eventDate";
        internal const string GetCountOfTicketsReserved = "SELECT " +
                                                            "COUNT(c.quantity) as countOf, c.reservationStatus, c.seatingType " +
                                                            "FROM c " +
                                                            "WHERE c.eventId = @eventId " +
                                                            "GROUP BY c.reservationStatus, c.seatingType";
        internal const string GetAllByEventId = "SELECT * FROM c WHERE c.EventId = @eventId ORDER BY c.EventDate DESC";
    }
}
