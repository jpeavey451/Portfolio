using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using TicketDepot.Shared;
using TicketDepot.TicketManagement.Repository;
using Validation;

namespace TicketDepot.TicketManagement.Domain
{
    /// <summary>
    /// The Event Povider class.
    /// </summary>
    public class EventProvider : IEventProvider
    {
        private readonly ILogger<EventProvider> logger;
        private readonly IEventRepository eventRepository;
        private readonly IObjectResultProvider objectResultProvider;
        private readonly IReservationRepository reservationRepository;
        private readonly IPaymentProvider paymentProvider;
        private readonly IEventValidator eventValidator;

        /// <summary>
        /// Initializes a new instance of <see cref="EventProvider"/>.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="objectResultProvider"></param>
        /// <param name="eventRepository"></param>
        /// <param name="reservationRepository"></param>
        /// <param name="paymentProvider"></param>
        public EventProvider(
            ILogger<EventProvider> logger,
            IObjectResultProvider objectResultProvider,
            IEventRepository eventRepository,
            IReservationRepository reservationRepository,
            IPaymentProvider paymentProvider,
            IEventValidator eventValidator)
        {
            this.logger = logger;
            this.eventRepository = eventRepository;
            this.objectResultProvider = objectResultProvider;
            this.reservationRepository = reservationRepository;
            this.paymentProvider = paymentProvider;
            this.eventValidator = eventValidator;
        }

        /// <inheritdoc/>
        public async Task<ObjectResult> CancelTicketsAsync(string reservationId, string transactionId, CancellationToken cancellationToken = default)
        {
            try
            {
                ObjectResult validationResult = await this.eventValidator.CancelTicketsValidation(reservationId, transactionId, cancellationToken).ConfigureAwait(false);
                if (validationResult.StatusCode != (int)HttpStatusCode.OK)
                {
                    return validationResult;
                }

                if (!(validationResult.Value is Reservation currentReservation))
                {
                    throw new InvalidOperationException("A50DF9CB-6528-45D7-9892-A0F718E604B2 Unexpected Error. Reservation is null.");
                }

                switch (currentReservation.ReservationStatus)
                {
                    case ReservationStatus.Cancelled:
                        {
                            return this.objectResultProvider.Ok();
                        }
                    case ReservationStatus.Purchased:
                        {
                            return this.objectResultProvider.PreconditionFailed("Purchased tickets cannot be cancelled");
                        }
                    case ReservationStatus.Reserved:
                    default:
                        {
                            currentReservation.ReservationStatus = ReservationStatus.Cancelled;
                            ObjectResult cancelResult = await this.reservationRepository.UpdateAsync(reservationId, currentReservation).ConfigureAwait(false);
                            return cancelResult;
                        }
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "4F252C9B-37B5-4F62-8AB3-1136A6D81F4E Failed to cancel Tickets.");
                return this.objectResultProvider.ServerError();
            }
        }

        /// <inheritdoc/>
        public async Task<ObjectResult> CreateEventAsync(Event newEvent, CancellationToken cancellationToken = default)
        {
            try
            {
                ObjectResult validationResult = await this.eventValidator.ValidateNewEvent(newEvent, cancellationToken).ConfigureAwait(false);
                if (validationResult.StatusCode != (int)HttpStatusCode.OK)
                {
                    return validationResult;
                }

                ObjectResult createResult = await this.eventRepository.AddAsync(newEvent, cancellationToken).ConfigureAwait(false);
                return createResult;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "3339A84C-959A-4976-A1B7-132F0A72F2AE Failed to create new Event.");
                return this.objectResultProvider.ServerError();
            }
        }

        /// <inheritdoc/>
        public async Task<ObjectResult> GetAvailabilityAsync(string eventId, CancellationToken cancellationToken = default)
        {
            try
            {
                ObjectResult validationResult = await this.eventValidator.ValidateGetAvailability(eventId, cancellationToken).ConfigureAwait(false);
                if (validationResult.StatusCode != (int)HttpStatusCode.OK)
                {
                    return validationResult;
                }

                (Event? currentEvent, Venue? currentVenue, List<TicketTypeAvailability>? ticketTypes) = (Tuple<Event, Venue, List<TicketTypeAvailability>>)validationResult?.Value!;
                if (currentEvent is null)
                {
                    throw new InvalidDataException("AA586D8B-6262-4AF9-95AB-E59CFEAC58FD Unexpected Error. Event is null");
                }

                if (currentVenue is null)
                {
                    throw new InvalidDataException("E06CCAFC-37B3-49B4-BF96-E815901D29A1 Unexpected Error. Venue is null");
                }

                if (ticketTypes is null)
                {
                    throw new InvalidDataException("FAD0025B-56CA-408F-B05F-1ACB0A23EF15 Unexpected Error. TicketTypes is null");
                }

                return this.objectResultProvider.Ok(new { Event = currentEvent, Venue = currentVenue, Availability = ticketTypes });
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "83C8CA16-45EC-4CF9-8BD6-87FEAAB0E19B Failed to update existing Event.");
                return this.objectResultProvider.ServerError();
            }

        }

        /// <inheritdoc/>
        public async Task<ObjectResult> GetEventByNameAsync(string eventName, CancellationToken cancellationToken = default)
        {
            try
            {
                Requires.NotNullOrWhiteSpace(eventName, nameof(eventName));

                ObjectResult result = await this.eventRepository.GetByNameAsync<Event>(eventName, cancellationToken).ConfigureAwait(false);
                return result;
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex, "F268E489-AB30-49BB-8166-4DD43523350F Failed to get Event.");
                return this.objectResultProvider.ServerError();
            }
        }

        /// <inheritdoc/>
        public async Task<ObjectResult> PurchaseTicketsAsync(Reservation reservation, CancellationToken cancellationToken = default)
        {
            try
            {
                ObjectResult validationResult = await this.eventValidator.ValidatePurchaseTicketsAsync(reservation, cancellationToken).ConfigureAwait(false);
                if (validationResult.StatusCode != (int)HttpStatusCode.OK)
                {
                    return validationResult;
                }

                ObjectResult paymentCheckResult = await this.paymentProvider.ProcessPayment(reservation?.PaymentInfo!, cancellationToken).ConfigureAwait(false);
                if (paymentCheckResult.StatusCode != (int)HttpStatusCode.OK)
                {
                    return paymentCheckResult;
                }

                reservation!.ReservationStatus = ReservationStatus.Purchased;
                ObjectResult createReservationResult = await this.reservationRepository.AddAsync(reservation!, cancellationToken).ConfigureAwait(false);
                return createReservationResult;

            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "8D271DAB-47F0-49DE-AEC9-C614581EA491 Failed to reserve Tickets.");
                return this.objectResultProvider.ServerError();
            }
        }

        /// <inheritdoc/>
        public async Task<ObjectResult> ReserveTicketsAsync(Reservation reservation, CancellationToken cancellationToken = default)
        {
            try
            {
                ObjectResult validationResult = await this.eventValidator.ValidateReserveTicket(reservation, cancellationToken).ConfigureAwait(false);
                if (validationResult.StatusCode != (int)HttpStatusCode.OK)
                {
                    return validationResult;
                }

                ObjectResult createReservationResult = await this.reservationRepository.AddAsync(reservation, cancellationToken).ConfigureAwait(false);
                return createReservationResult;

            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "8D271DAB-47F0-49DE-AEC9-C614581EA491 Failed to reserve Tickets.");
                return this.objectResultProvider.ServerError();
            }
        }

        /// <inheritdoc/>
        public async Task<ObjectResult> UpdateEventAsync(Event updatedEvent, CancellationToken cancellationToken)
        {
            try
            {
                ObjectResult validationResult = await this.eventValidator.ValidateUpdatedEvent(updatedEvent, cancellationToken).ConfigureAwait(false);
                if (validationResult.StatusCode != (int)HttpStatusCode.OK)
                {
                    return validationResult;
                }

                Event? existingEvent = validationResult.Value! as Event;
                updatedEvent.CreatedDate = (DateTimeOffset)existingEvent?.CreatedDate!;

                ObjectResult updateResult = await this.eventRepository.UpdateAsync(updatedEvent.Id!, updatedEvent, cancellationToken).ConfigureAwait(false);
                return updateResult;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "83C8CA16-45EC-4CF9-8BD6-87FEAAB0E19B Failed to update existing Event.");
                return this.objectResultProvider.ServerError();
            }
        }
    }
}
