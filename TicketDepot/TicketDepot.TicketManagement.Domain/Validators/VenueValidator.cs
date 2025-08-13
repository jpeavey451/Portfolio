using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using TicketDepot.Shared;
using TicketDepot.TicketManagement.Repository;
using Validation;

namespace TicketDepot.TicketManagement.Domain
{
    /// <summary>
    /// The Venue Validator class.
    /// </summary>
    public class VenueValidator : IVenueValidator
    {
        private readonly IObjectResultProvider objectResultProvider;
        private readonly IValidationProvider validationProvider;
        private readonly IVenueRepository venueRepository;

        public VenueValidator(
            IVenueRepository venueRepository,
            IObjectResultProvider objectResultProvider,
            IValidationProvider validationProvider)
        {
            this.venueRepository = venueRepository;
            this.objectResultProvider = objectResultProvider;
            this.validationProvider = validationProvider;
        }

        /// <inheritdoc/>
        public async Task<ObjectResult> ValidateNewVenu(Venue newVenue, CancellationToken cancellationToken = default)
        {
            Requires.NotNull(newVenue, nameof(newVenue));

            ObjectResult validationResult = this.validationProvider.ValidateObject(newVenue);
            if (validationResult.StatusCode != (int)HttpStatusCode.OK)
            {
                return validationResult;
            }

            ObjectResult dupeCheckResult = await this.venueRepository.GetByNameAsync<Venue>(newVenue.Name, cancellationToken).ConfigureAwait(false);
            if (dupeCheckResult.StatusCode != (int)HttpStatusCode.OK && dupeCheckResult.StatusCode != (int)HttpStatusCode.NotFound)
            {
                return dupeCheckResult;
            }

            if (dupeCheckResult.Value is List<Venue> existingVenues)
            {
                return this.objectResultProvider.RecordExists(existingVenues);
            }

            return this.objectResultProvider.Ok();
        }

        /// <inheritdoc/>
        public async Task<ObjectResult> ValidateUpdateVenue(Venue updatedVenue, CancellationToken cancellationToken = default)
        {
            Requires.NotNull(updatedVenue, nameof(updatedVenue));

            ObjectResult validationResult = this.validationProvider.ValidateObject(updatedVenue);
            if (validationResult.StatusCode != (int)HttpStatusCode.OK)
            {
                return validationResult;
            }

            ObjectResult dupeCheckResult = await this.venueRepository.GetByNameAsync<Venue>(updatedVenue.Name, cancellationToken).ConfigureAwait(false);
            if (dupeCheckResult.StatusCode != (int)HttpStatusCode.OK)
            {
                return dupeCheckResult;
            }

            List<Venue> existingVenues = dupeCheckResult.Value as List<Venue> ?? new List<Venue>();
            IEnumerable<Venue> duplicateVenues = existingVenues.Where(v => v.Name.Equals(updatedVenue.Name, StringComparison.OrdinalIgnoreCase)
                                                                        && !v.Id.Equals(updatedVenue.Id, StringComparison.Ordinal));
            if (duplicateVenues.Count() > 0)
            {
                return this.objectResultProvider.RecordExists(new KnownError<IEnumerable<Venue>>("You cannot change a Venues name to an existing Venue.", duplicateVenues));
            }

            Venue? existingVenue = existingVenues.FirstOrDefault(v => v.Id.Equals(updatedVenue.Id, StringComparison.Ordinal));
            updatedVenue.CreatedDate = existingVenue?.CreatedDate ?? DateTimeOffset.UtcNow;
            return this.objectResultProvider.Ok();
        }
    }
}
