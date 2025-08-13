using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketDepot.TicketManagement.Repository;

namespace TicketDepot.TicketManagement.Domain.Tests.Factory
{
    internal static class DomainFactory
    {
        static string transactionId = Guid.NewGuid().ToString("N");
        static string reservationId = Guid.NewGuid().ToString("N");
        static string ticketTypeId = Guid.NewGuid().ToString("N");
        static string eventId = Guid.NewGuid().ToString("N");
        static string venueId = Guid.NewGuid().ToString("N");
        static string customerAccountNumber = Guid.NewGuid().ToString("N");
        static string eventName = "Jon Bon Jovi @ Carnegie Hall";

        internal static Reservation GetReservation()
        {
            return new Reservation(
                GetPaymentInfo(),
                transactionId,
                eventId,
                ticketTypeId,
                customerAccountNumber,
                SeatingType.VIP,
                15,
                5,
                ReservationStatus.Reserved);
        }

        internal static PaymentInfo GetPaymentInfo()
        {
            return new PaymentInfo(
                customerAccountNumber,
                transactionId,
                (decimal)new Random().NextDouble(),
                Guid.NewGuid().ToString("N"),
                "11/30",
                "555",
                null,
                null);
        }

        internal static Event GetEvent()
        {
            return new Event(
                eventName,
                venueId,
                "Woot JON BON JOVI!",
                EventType.Concert,
                new DateOnly(2025, 12, 31),
                new TimeOnly(1900, 0));
        }
    }
}
