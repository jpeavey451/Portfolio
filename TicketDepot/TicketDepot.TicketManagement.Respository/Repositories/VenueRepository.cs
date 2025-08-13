

using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using System.Net;
using TicketDepot.Shared;
using Validation;

namespace TicketDepot.TicketManagement.Repository
{
    public class VenueRepository : IVenueRepository
    {
        private readonly IContainerRepository<IVenueContainer> containerRepository;
        private readonly ILogger<VenueRepository> logger;
        private readonly IObjectResultProvider objectResultProvider;
        private readonly IValidationProvider validationProvider;

        public VenueRepository(
            ILogger<VenueRepository> logger,
            IContainerRepository<IVenueContainer> containerRepository,
            IObjectResultProvider objectResultProvider,
            IValidationProvider validationProvider)
        {
            this.containerRepository = containerRepository;
            this.logger = logger;
            this.objectResultProvider = objectResultProvider;
            this.validationProvider = validationProvider;
        }

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
                this.logger.LogError(ex, "D2D4421F-9401-4DAA-8B74-410D8EF7749E Failed to get add new Venue.");
                return this.objectResultProvider.ServerError();
            }
        }

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
                this.logger.LogError(ex, "D6D54EB1-9740-4B64-91D2-E9C155B083F9 Failed to get all Venues.");
                return this.objectResultProvider.ServerError();
            }
        }

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
                this.logger.LogError(ex, "72A91E25-64D2-44E3-8BFB-AAB2D5C686EE Failed to get existing Venue.");
                return this.objectResultProvider.ServerError();
            }
        }

        public async Task<ObjectResult> GetByNameAsync<T>(string name, CancellationToken cancellationToken = default) where T : class
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
                this.logger.LogError(ex, "DB166E86-72E1-44CE-8A48-E1F16D18DD94 Failed to get existing Venue.");
                return this.objectResultProvider.ServerError();
            }
        }

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
                this.logger.LogError(ex, "D2D4421F-9401-4DAA-8B74-410D8EF7749E Failed to update Venue.");
                return this.objectResultProvider.ServerError();
            }
        }
    }
}
