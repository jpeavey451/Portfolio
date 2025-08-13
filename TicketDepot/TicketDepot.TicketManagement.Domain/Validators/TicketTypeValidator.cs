using Microsoft.AspNetCore.Mvc;
using System.Net;
using TicketDepot.Shared;
using TicketDepot.TicketManagement.Repository;
using Validation;

namespace TicketDepot.TicketManagement.Domain
{
    /// <summary>
    /// The TicketType Validator class
    /// </summary>
    public class TicketTypeValidator : ITicketTypeValidator
    {
        private readonly ITicketTypeRepository ticketTypeRepository;
        private readonly IEventRepository eventRepository;
        private readonly IVenueRepository venueRepository;
        private readonly IObjectResultProvider objectResultProvider;
        private readonly IValidationProvider validationProvider;

        /// <summary>
        /// Initializes a new instance of <see cref="TicketTypeProvider"/>.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="objectResultProvider"></param>
        /// <param name="ticketTypeRepository"></param>
        /// <param name="eventRepository"></param
        /// <param name="IVenueRepository"></param>
        /// <param name="validationProvider"></param>
        public TicketTypeValidator(
            IObjectResultProvider objectResultProvider,
            ITicketTypeRepository ticketTypeRepository,
            IEventRepository eventRepository,
            IVenueRepository venueRepository,
            IValidationProvider validationProvider)
        {
            this.ticketTypeRepository = ticketTypeRepository;
            this.eventRepository = eventRepository;
            this.venueRepository = venueRepository;
            this.objectResultProvider = objectResultProvider;
            this.validationProvider = validationProvider;
        }

        /// <inheritdoc/>
        public async Task<ObjectResult> ValidateNewTicketType(TicketType newTicketType, CancellationToken cancellationToken = default)
        {
            Requires.NotNull(newTicketType, nameof(newTicketType));

            ObjectResult validationResult = this.validationProvider.ValidateObject(newTicketType);
            if (validationResult.StatusCode != (int)HttpStatusCode.OK)
            {
                return validationResult;
            }

            // Event Check
            ObjectResult eventValidationResult = await this.eventRepository.GetByIdAsync<Event>(newTicketType.EventId, cancellationToken).ConfigureAwait(false);
            if (eventValidationResult.StatusCode != (int)HttpStatusCode.OK)
            {
                return eventValidationResult;
            }

            if (!(eventValidationResult?.Value is Event currentEvent))
            {
                throw new InvalidDataException("AA863582-A157-4C61-9A9C-AF2FFF5D2419 Unexpected Error. Event is null");
            }

            // Venue Check
            ObjectResult venueValidationResult = await this.venueRepository.GetByIdAsync<Event>(newTicketType.VenueId, cancellationToken).ConfigureAwait(false);
            if (venueValidationResult.StatusCode != (int)HttpStatusCode.OK)
            {
                return venueValidationResult;
            }

            if (!(venueValidationResult?.Value is Venue currentVenue))
            {
                throw new InvalidDataException("FD8E9F28-2947-4CE5-9B39-3018F2AC77A7 Unexpected Error. Venue is null");
            }

            // Get existing TicketTypes
            ObjectResult capacityValidationResult = await this.ticketTypeRepository.GetAllByEventIdAsync<TicketType>(eventId: newTicketType.EventId, cancellationToken: cancellationToken).ConfigureAwait(false);
            if (venueValidationResult.StatusCode != (int)HttpStatusCode.OK && capacityValidationResult.StatusCode != (int)HttpStatusCode.NotFound)
            {
                return capacityValidationResult;
            }

            List<TicketType> ticketTypes = capacityValidationResult?.Value! as List<TicketType> ?? new List<TicketType>();

            // Duplicate check
            if (ticketTypes.FirstOrDefault(t => t.SeatingType == newTicketType.SeatingType) != null)
            {
                return this.objectResultProvider.RecordExists(new KnownError<List<TicketType>>($"Duplicate SeatingType specified [{newTicketType.SeatingType}]", ticketTypes));
            }

            ticketTypes.Add(newTicketType);

            // Capacity check
            int totalAllocatedCapacity = ticketTypes.Sum(t => t.Capacity);
            if (currentVenue.MaxCapacity < totalAllocatedCapacity)
            {
                return this.objectResultProvider.BadRequest(new KnownError<List<TicketType>>($"Total allocated capacity: [{totalAllocatedCapacity}] would exceed Venue Max Capacity: [{currentVenue.MaxCapacity}]", ticketTypes));
            }

            return this.objectResultProvider.Ok();
        }

        /// <inheritdoc/>
        public async Task<ObjectResult> ValidateUpdatedTicketType(TicketType updatedTicketType, CancellationToken cancellationToken = default)
        {
            Requires.NotNull(updatedTicketType, nameof(updatedTicketType));

            ObjectResult validationResult = this.validationProvider.ValidateObject(updatedTicketType);
            if (validationResult.StatusCode != (int)HttpStatusCode.OK)
            {
                return validationResult;
            }

            // Event Check
            ObjectResult eventCheckResult = await this.eventRepository.GetByIdAsync<Event>(updatedTicketType.EventId, cancellationToken).ConfigureAwait(false);
            if (eventCheckResult.StatusCode != (int)HttpStatusCode.OK)
            {
                return eventCheckResult;
            }

            if (!(eventCheckResult?.Value is Event currentEvent))
            {
                throw new InvalidDataException("8E206E1E-C4CB-4694-9CC3-4B9661EA6599 Unexpected Error. Event is null");
            }

            // Venue Check
            ObjectResult venueCheckResult = await this.venueRepository.GetByIdAsync<Event>(updatedTicketType.VenueId, cancellationToken).ConfigureAwait(false);
            if (venueCheckResult.StatusCode != (int)HttpStatusCode.OK)
            {
                return venueCheckResult;
            }

            if (!(venueCheckResult?.Value is Venue currentVenue))
            {
                throw new InvalidDataException("19E51592-FD66-4D46-80A5-7969926D29CD Unexepected Error. Venue is null");
            }

            // Get existing TicketTypes
            ObjectResult capacityCheckResult = await this.ticketTypeRepository.GetAllByEventIdAsync<TicketType>(eventId: updatedTicketType.EventId, cancellationToken: cancellationToken).ConfigureAwait(false);
            if (capacityCheckResult.StatusCode != (int)HttpStatusCode.OK)
            {
                return capacityCheckResult;
            }

            List<TicketType> ticketTypes = capacityCheckResult?.Value! as List<TicketType> ?? new List<TicketType>();

            // Existence check
            TicketType? existingTicketType = ticketTypes.FirstOrDefault(t => t.SeatingType == updatedTicketType.SeatingType);
            if (existingTicketType == null)
            {
                return this.objectResultProvider.ResourceNotFound();
            }

            existingTicketType.Capacity = updatedTicketType.Capacity;

            // Capacity check
            int totalAllocatedCapacity = ticketTypes.Sum(t => t.Capacity);
            if (currentVenue.MaxCapacity < totalAllocatedCapacity)
            {
                return this.objectResultProvider.BadRequest(new KnownError<List<TicketType>>($"Total allocated capacity: [{totalAllocatedCapacity}] would exceed Venue Max Capacity: [{currentVenue.MaxCapacity}]", ticketTypes));
            }

            return this.objectResultProvider.Ok();
        }
    }
}
