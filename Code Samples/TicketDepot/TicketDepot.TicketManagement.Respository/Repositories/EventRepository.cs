using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using System.Net;
using TicketDepot.Shared;
using Validation;

namespace TicketDepot.TicketManagement.Repository
{
    /// <summary>
    /// The repository class for <see cref="Event"/>.
    /// </summary>
    public class EventRepository : IEventRepository
    {
        private readonly IContainerRepository<IEventContainer> containerRepository;
        private readonly ILogger<EventRepository> logger;
        private readonly IObjectResultProvider objectResultProvider;
        private readonly IValidationProvider validationProvider;

        public EventRepository(
            ILogger<EventRepository> logger,
            IContainerRepository<IEventContainer> containerRepository,
            IObjectResultProvider objectResultProvider,
            IValidationProvider validationProvider)
        {
            this.containerRepository = containerRepository;
            this.logger = logger;
            this.objectResultProvider = objectResultProvider;
            this.validationProvider = validationProvider;
        }

        /// <inheritdoc/>
        public async Task<ObjectResult> AddAsync<T>(T item, CancellationToken cancellationToken = default)
            where T : class
        {
            try
            {
                Requires.NotNull(item, nameof(item));

                ObjectResult validationResult = this.validationProvider.ValidateObject(item);
                if (validationResult.StatusCode != (int)HttpStatusCode.OK)
                {
                    return validationResult;
                }

                ObjectResult addResult = await this.containerRepository.AddItemAsync(item).ConfigureAwait(false);
                return addResult;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "E697C9F3-A4D3-4F34-9CEE-A6EBABD25BB5 Failed to add new Event.");
                return this.objectResultProvider.ServerError();
            }
        }


        /// <inheritdoc/>
        public async Task<ObjectResult> GetAllAsync<T>(string continuationToken, CancellationToken cancellationToken = default) where T : class
        {
            try
            {
                QueryDefinition queryDefinition = new QueryDefinition(Queries.GetAllItems);

                ObjectResult response = await this.containerRepository.GetItemsAsync<T>(queryDefinition, continuationToken, cancellationToken);
                if (response.StatusCode != (int)HttpStatusCode.OK)
                {
                    return response;
                }

                return this.objectResultProvider.Ok(response);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "D6D54EB1-9740-4B64-91D2-E9C155B083F9 Failed to get all Events.");
                return this.objectResultProvider.ServerError();
            }
        }

        /// <inheritdoc/>
        public async Task<ObjectResult> GetByIdAsync<T>(string id, CancellationToken cancellationToken = default)
            where T : class
        {
            try
            {
                Requires.NotNullOrWhiteSpace(id, nameof(id));

                QueryDefinition queryDefinition = new QueryDefinition(Queries.GetById)
                    .WithParameter(QueryParams.Id, id);

                ObjectResult response = await this.containerRepository.GetItemsAsync<T>(queryDefinition);
                if (response.StatusCode != (int)HttpStatusCode.OK)
                {
                    return response;
                }

                List<T>? results = response.Value! as List<T> ?? new List<T>();
                T? existingEvent = results.FirstOrDefault();

                if (existingEvent is null)
                {
                    return this.objectResultProvider.ResourceNotFound();
                }

                return this.objectResultProvider.Ok(existingEvent);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "B7C15A6C-998C-488C-ACC1-8D7E4939F68B Failed to get existing Venue.");
                return this.objectResultProvider.ServerError();
            }
        }

        /// <inheritdoc/>
        public async Task<ObjectResult> GetByNameAsync<T>(string name, CancellationToken cancellationToken = default)
            where T : class
        {
            try
            {
                Requires.NotNullOrWhiteSpace(name, nameof(name));

                QueryDefinition queryDefinition = new QueryDefinition(Queries.GetByName)
                    .WithParameter(QueryParams.Name, name);

                ObjectResult response = await this.containerRepository.GetItemsAsync<T>(queryDefinition);
                if (response.StatusCode != (int)HttpStatusCode.OK)
                {
                    return response;
                }

                List<T>? results = response.Value! as List<T> ?? new List<T>();
                if (!results.Any())
                {
                    return this.objectResultProvider.ResourceNotFound();
                }

                return this.objectResultProvider.Ok(results);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "CF4289BA-5DD9-4318-9B3C-A379B895521B Failed to get existing Event.");
                return this.objectResultProvider.ServerError();
            }
        }

        /// <inheritdoc/>
        public async Task<ObjectResult> GetEventsByVenueIdAndDate(string venueId, DateOnly eventDate, CancellationToken cancellationToken = default)
        {
            try
            {
                Requires.NotNullOrWhiteSpace(venueId, nameof(venueId));

                QueryDefinition queryDefinition = new QueryDefinition(Queries.GetEventByVenueIdAndDate)
                    .WithParameter(QueryParams.VenueId, venueId)
                    .WithParameter(QueryParams.EventDate, eventDate);

                ObjectResult response = await this.containerRepository.GetItemsAsync<Event>(queryDefinition);
                if (response.StatusCode != (int)HttpStatusCode.OK)
                {
                    return response;
                }

                List<Event>? results = response.Value! as List<Event> ?? new List<Event>();
                if (!results.Any())
                {
                    return this.objectResultProvider.ResourceNotFound();
                }

                return this.objectResultProvider.Ok(results);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "CF4289BA-5DD9-4318-9B3C-A379B895521B Failed to get existing Event.");
                return this.objectResultProvider.ServerError();
            }
        }

        /// <inheritdoc/>
        public async Task<ObjectResult> UpdateAsync<T>(string id, T item, CancellationToken cancellationToken = default)
            where T : class
        {
            try
            {
                Requires.NotNull(item, nameof(item));

                ObjectResult validationResult = this.validationProvider.ValidateObject(item);
                if (validationResult.StatusCode != (int)HttpStatusCode.OK)
                {
                    return validationResult;
                }

                ObjectResult addResult = await this.containerRepository.UpdateItemAsync(id, item).ConfigureAwait(false);
                return addResult;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "FCA4B92A-A35B-42FD-B28D-6E10C6E32FF7 Failed to update Event.");
                return this.objectResultProvider.ServerError();
            }
        }
    }
}
