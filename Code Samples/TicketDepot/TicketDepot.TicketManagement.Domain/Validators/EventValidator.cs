
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TicketDepot.Shared;
using TicketDepot.TicketManagement.Repository;
using Validation;

namespace TicketDepot.TicketManagement.Domain
{
    public class EventValidator : IEventValidator
    {
        private readonly IValidationProvider validationProvider;
        private readonly IEventRepository eventRepository;
        private readonly IObjectResultProvider objectResultProvider;
        private readonly IVenueRepository venueRepository;
        private readonly ITransactionRepository transactionRepository;
        private readonly ICustomerRespository customerRespository;
        private readonly IReservationRepository reservationRepository;
        private readonly ITicketTypeRepository ticketTypeRepository;

        public EventValidator(
            IValidationProvider validationProvider,
            IEventRepository eventRepository,
            IVenueRepository venueRepository,
            ITransactionRepository transactionRepository,
            ICustomerRespository customerRespository,
            IReservationRepository reservationRepository,
            ITicketTypeRepository ticketTypeRepository,
            IObjectResultProvider objectResultProvider)
        {
            this.validationProvider = validationProvider;
            this.eventRepository = eventRepository;
            this.objectResultProvider = objectResultProvider;
            this.venueRepository = venueRepository;
            this.transactionRepository = transactionRepository;
            this.customerRespository = customerRespository;
            this.ticketTypeRepository = ticketTypeRepository;
            this.reservationRepository = reservationRepository;
        }

        public async Task<ObjectResult> ValidateNewEvent(Event newEvent, CancellationToken cancellationToken = default)
        {
            Requires.NotNull(newEvent, nameof(newEvent));

            ObjectResult validationResult = this.validationProvider.ValidateObject(newEvent);
            if (validationResult.StatusCode != (int)HttpStatusCode.OK)
            {
                return validationResult;
            }

            ObjectResult dupeCheckResult = await this.eventRepository.GetByNameAsync<Event>(newEvent.Name, cancellationToken).ConfigureAwait(false);
            if (dupeCheckResult.StatusCode != (int)HttpStatusCode.OK && dupeCheckResult.StatusCode != (int)HttpStatusCode.NotFound)
            {
                return dupeCheckResult;
            }

            if (dupeCheckResult.Value is Event existingEvent)
            {
                return this.objectResultProvider.RecordExists(existingEvent);
            }

            ObjectResult venueCheckResult = await this.venueRepository.GetByIdAsync<Venue>(newEvent.VenueId, cancellationToken).ConfigureAwait(false);
            if (venueCheckResult.StatusCode == (int)HttpStatusCode.NotFound)
            {
                return this.objectResultProvider.ResourceNotFound($"Venue with Id [{newEvent.VenueId}] not found.");
            }

            if (venueCheckResult.StatusCode != (int)HttpStatusCode.OK)
            {
                return venueCheckResult;
            }

            return this.objectResultProvider.Ok();
        }

        public async Task<ObjectResult> ValidateUpdatedEvent(Event updatedEvent, CancellationToken cancellationToken = default)
        {
            Requires.NotNull(updatedEvent, nameof(updatedEvent));

            ObjectResult validationResult = this.validationProvider.ValidateObject(updatedEvent);
            if (validationResult.StatusCode != (int)HttpStatusCode.OK)
            {
                return validationResult;
            }


            ObjectResult dupeCheckResult = await this.eventRepository.GetByNameAsync<Event>(updatedEvent.Name, cancellationToken).ConfigureAwait(false);
            if (dupeCheckResult.StatusCode == (int)HttpStatusCode.NotFound)
            {
                return this.objectResultProvider.ResourceNotFound($"Event with Id [{updatedEvent.Id}] not found.");
            }

            if (dupeCheckResult.StatusCode != (int)HttpStatusCode.OK)
            {
                return dupeCheckResult;
            }

            List<Event> existingEvents = dupeCheckResult.Value as List<Event> ?? new List<Event>();
            Event? existingEvent = existingEvents.FirstOrDefault();
            if (existingEvent is null)
            {
                throw new InvalidDataException("2F1A84E1-DEC0-422F-B80E-A246381F30B6 Unexpected Error. Event is null");
            }

            ObjectResult venueCheckResult = await this.venueRepository.GetByIdAsync<Venue>(updatedEvent.VenueId, cancellationToken).ConfigureAwait(false);
            if (venueCheckResult.StatusCode == (int)HttpStatusCode.NotFound)
            {
                return this.objectResultProvider.ResourceNotFound($"Venue with Id [{updatedEvent.VenueId}] not found.");
            }

            if (venueCheckResult.StatusCode != (int)HttpStatusCode.OK)
            {
                return venueCheckResult;
            }

            ObjectResult venueBookedCheckResult = await this.eventRepository.GetEventsByVenueIdAndDate(updatedEvent.VenueId, updatedEvent.EventDate, cancellationToken).ConfigureAwait(false);
            if (venueBookedCheckResult.StatusCode != (int)HttpStatusCode.OK && venueBookedCheckResult.StatusCode != (int)HttpStatusCode.NotFound)
            {
                return venueBookedCheckResult;
            }

            List<Event> duplicateEvents = venueBookedCheckResult.Value as List<Event> ?? new List<Event>();
            if (duplicateEvents.Any())
            {
                this.objectResultProvider.RecordExists(new KnownError<List<Event>>("You cannot book more than one Event for a Venue per day.", duplicateEvents));
            }

            return this.objectResultProvider.Ok(existingEvent);
        }

        public async Task<ObjectResult> ValidateReserveTicket(Reservation reservation, CancellationToken cancellationToken = default)
        {

            Requires.NotNull(reservation, nameof(reservation));

            ObjectResult validationResult = this.validationProvider.ValidateObject(reservation);
            if (validationResult.StatusCode != (int)HttpStatusCode.OK)
            {
                return validationResult;
            }

            ObjectResult transactionCheckResult = await this.transactionRepository.GetByIdAsync<Transaction>(reservation.TransactionId, cancellationToken).ConfigureAwait(false);
            if (transactionCheckResult.StatusCode != (int)HttpStatusCode.OK && transactionCheckResult.StatusCode != (int)HttpStatusCode.NotFound)
            {
                return transactionCheckResult;
            }

            if (transactionCheckResult.StatusCode == (int)HttpStatusCode.OK)
            {
                Transaction? transaction = transactionCheckResult?.Value as Transaction;
                if (transaction is not null)
                {
                    return this.objectResultProvider.AlreadyReported();
                }
            }

            ObjectResult customerCheckResult = await this.customerRespository.GetByIdAsync<Customer>(reservation.CustomerAccountNumber, cancellationToken).ConfigureAwait(false);
            if (customerCheckResult.StatusCode != (int)HttpStatusCode.OK)
            {
                return customerCheckResult;
            }

            ObjectResult eventCheckResult = await this.eventRepository.GetByIdAsync<Event>(reservation.EventId, cancellationToken).ConfigureAwait(false);
            if (eventCheckResult.StatusCode != (int)HttpStatusCode.OK)
            {
                return eventCheckResult;
            }

            return this.objectResultProvider.Ok();
        }

        public async Task<ObjectResult> ValidatePurchaseTicketsAsync(Reservation reservation, CancellationToken cancellationToken = default)
        {
            Requires.NotNull(reservation, nameof(reservation));

            ObjectResult reservationValidationResult = this.validationProvider.ValidateObject(reservation);
            if (reservationValidationResult.StatusCode != (int)HttpStatusCode.OK)
            {
                return reservationValidationResult;
            }

            ObjectResult transactionCheckResult = await this.transactionRepository.GetByIdAsync<Transaction>(reservation?.TransactionId!, cancellationToken).ConfigureAwait(false);
            if (transactionCheckResult.StatusCode != (int)HttpStatusCode.OK && transactionCheckResult.StatusCode != (int)HttpStatusCode.NotFound)
            {
                return transactionCheckResult;
            }

            if (transactionCheckResult.StatusCode == (int)HttpStatusCode.OK)
            {
                Transaction? transaction = transactionCheckResult?.Value as Transaction;
                if (transaction is not null && transaction.IsSuccess)
                {
                    return this.objectResultProvider.AlreadyReported();
                }
            }

            ObjectResult customerCheckResult = await this.customerRespository.GetByIdAsync<Customer>(reservation?.CustomerAccountNumber!, cancellationToken).ConfigureAwait(false);
            if (customerCheckResult.StatusCode != (int)HttpStatusCode.OK)
            {
                return customerCheckResult;
            }

            ObjectResult eventCheckResult = await this.eventRepository.GetByIdAsync<Event>(reservation?.EventId!, cancellationToken).ConfigureAwait(false);
            if (eventCheckResult.StatusCode != (int)HttpStatusCode.OK)
            {
                return eventCheckResult;
            }

            return this.objectResultProvider.Ok();
        }

        public async Task<ObjectResult> ValidateGetAvailability(string eventId, CancellationToken cancellationToken = default)
        {

            Requires.NotNullOrWhiteSpace(eventId, nameof(eventId));

            ObjectResult eventCheckResult = await this.eventRepository.GetByIdAsync<Event>(eventId, cancellationToken).ConfigureAwait(false);
            if (eventCheckResult.StatusCode == (int)HttpStatusCode.NotFound)
            {
                return this.objectResultProvider.ResourceNotFound($"Event with Id [{eventId}] not found.");
            }

            if (eventCheckResult.StatusCode != (int)HttpStatusCode.OK)
            {
                return eventCheckResult;
            }

            if (!(eventCheckResult.Value is Event currentEvent))
            {
                throw new InvalidDataException("66460659-4EC9-4921-A886-34399BBFF164 Unexpected Error. Event is null.");
            }

            ObjectResult venueCheckResult = await this.venueRepository.GetByIdAsync<Venue>(currentEvent.VenueId, cancellationToken).ConfigureAwait(false);
            if (venueCheckResult.StatusCode == (int)HttpStatusCode.NotFound)
            {
                return this.objectResultProvider.ResourceNotFound($"Venue with Id [{currentEvent.VenueId}] not found.");
            }

            if (venueCheckResult.StatusCode != (int)HttpStatusCode.OK)
            {
                return venueCheckResult;
            }

            if (!(venueCheckResult.Value is Venue currentVenue))
            {
                throw new InvalidDataException("C792B3A4-1087-4AFB-BA01-2CDEC4E96110 Unexpected Error. Venue is null.");
            }

            // Get existing TicketTypes
            ObjectResult capacityCheckResult = await this.ticketTypeRepository.GetAllByEventIdAsync<TicketTypeAvailability>(eventId: eventId, cancellationToken: cancellationToken).ConfigureAwait(false);
            if (capacityCheckResult.StatusCode != (int)HttpStatusCode.OK)
            {
                return capacityCheckResult;
            }

            List<TicketTypeAvailability> ticketTypes = capacityCheckResult?.Value! as List<TicketTypeAvailability> ?? new List<TicketTypeAvailability>();
            if (!ticketTypes.Any())
            {
                return this.objectResultProvider.ResourceNotFound($"No TicketTypes found for Event Id: [{eventId}].");
            }

            ObjectResult ticketCountResult = await this.reservationRepository.GetCountOfTicketsReservedAsync(eventId, cancellationToken).ConfigureAwait(false);
            if (ticketCountResult.StatusCode != (int)HttpStatusCode.OK && ticketCountResult.StatusCode != (int)HttpStatusCode.NotFound)
            {
                return ticketCountResult;
            }

            List<TicketAvailability> ticketAvailabilities = ticketCountResult?.Value as List<TicketAvailability> ?? new List<TicketAvailability>();
            foreach (TicketTypeAvailability ticketTypeAvailability in ticketTypes)
            {
                // Purchased count
                TicketAvailability? ticketAvailability = ticketAvailabilities.FirstOrDefault(ta => ta.SeatingType == ticketTypeAvailability.SeatingType
                                                            && ta.ReservationStatus == ReservationStatus.Purchased)
                                                            ?? new TicketAvailability();

                ticketTypeAvailability.TotalTicketsPurchased = ticketAvailability.CountOf;

                // reserved cont
                ticketAvailability = ticketAvailabilities.FirstOrDefault(ta => ta.SeatingType == ticketTypeAvailability.SeatingType
                                                            && ta.ReservationStatus == ReservationStatus.Reserved)
                                                            ?? new TicketAvailability();

                ticketTypeAvailability.TotalTicketsReserved = ticketAvailability.CountOf;

                // total available
                ticketTypeAvailability.TotalAvailableTickets = ticketTypes.Sum(tt => tt.Capacity) - (ticketTypeAvailability.TotalTicketsPurchased);
            }

            return this.objectResultProvider.Ok((currentEvent, currentVenue, ticketTypes));

        }

        public async Task<ObjectResult> CancelTicketsValidation(string reservationId, string transactionId, CancellationToken cancellationToken = default)
        {
            Requires.NotNullOrWhiteSpace(reservationId, nameof(reservationId));

            ObjectResult transactionCheckResult = await this.transactionRepository.GetByIdAsync<Transaction>(transactionId, cancellationToken).ConfigureAwait(false);
            if (transactionCheckResult.StatusCode != (int)HttpStatusCode.OK && transactionCheckResult.StatusCode != (int)HttpStatusCode.NotFound)
            {
                return transactionCheckResult;
            }

            if (transactionCheckResult.StatusCode == (int)HttpStatusCode.OK)
            {
                Transaction? transaction = transactionCheckResult?.Value as Transaction;
                if (transaction is not null)
                {
                    return this.objectResultProvider.AlreadyReported();
                }
            }

            ObjectResult reservationCheckResult = await this.reservationRepository.GetByIdAsync<Reservation>(reservationId, cancellationToken).ConfigureAwait(false);
            if (reservationCheckResult.StatusCode != (int)HttpStatusCode.OK)
            {
                return reservationCheckResult;
            }

            if (!(reservationCheckResult.Value is Reservation currentReservation))
            {
                throw new InvalidDataException("085F7D42-218F-48D8-AAF2-491FA6FDB61D Unexpected Error. Reservation is null.");
            }

            return this.objectResultProvider.Ok(currentReservation);
        }
    }
}
