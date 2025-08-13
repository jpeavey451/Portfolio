using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using TicketDepot.Shared;

namespace TicketDepot.TicketManagement.Repository
{
    public class ContainerRepository<TR> : IContainerRepository<TR>
        where TR : ICosmosContainer
    {
        private readonly ICosmosContainer cosmosContainer;
        private readonly IObjectResultProvider objectResultsProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContainerRepository{TR}"/> class.
        /// </summary>
        /// <param name="cosmosContainer">The specific container.</param>
        public ContainerRepository(
            TR cosmosContainer,
            IObjectResultProvider objectResultsProvider)
        {
            this.cosmosContainer = cosmosContainer;
            this.objectResultsProvider = objectResultsProvider;
        }

        /// <inheritdoc/>
        public async Task<ObjectResult> AddItemAsync<T>(T item, CancellationToken cancellationToken = default)
        {
            if (this.cosmosContainer.Container is null)
            {
                throw new InvalidOperationException("3EA0A4C7-52EE-489A-AC4C-3FDA89CB1762 Unexpected Error. Container is null");
            }

            ItemResponse<T> response = await this.cosmosContainer.Container.CreateItemAsync<T>(item).ConfigureAwait(false);
            return this.objectResultsProvider.Ok(response.Resource);
        }

        /// <inheritdoc/>
        public async Task<ObjectResult> DeleteItemAsync<T>(string id, string partitionKey, CancellationToken cancellationToken = default)
        {
            if (this.cosmosContainer.Container is null)
            {
                throw new InvalidOperationException("A50DF9CB-6528-45D7-9892-A0F718E604B2 Unexpected Error. Container is null");
            }

            ItemResponse<T> response = await this.cosmosContainer.Container.DeleteItemAsync<T>(id, new PartitionKey(partitionKey), cancellationToken: cancellationToken).ConfigureAwait(false);
            return this.objectResultsProvider.Ok();
        }

        /// <inheritdoc/>
        public async Task<ObjectResult> GetItemAsync<T>(string id, string partitionKey, CancellationToken cancellationToken = default)
        {
            try
            {
                if (this.cosmosContainer.Container is null)
                {
                    throw new InvalidOperationException("29B3D4FE-6F00-4D34-B311-E0BA5799EC14 Unexpected Error. Container is null");
                }

                ItemResponse<T> response = await this.cosmosContainer.Container.ReadItemAsync<T>(id, new PartitionKey(partitionKey)).ConfigureAwait(false);
                return this.objectResultsProvider.Ok(response.Resource);
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return this.objectResultsProvider.ResourceNotFound();
            }
        }

        /// <inheritdoc/>
        public async Task<ObjectResult> GetItemsAsync<T>(QueryDefinition queryDefinition, string? continuationToken = null, CancellationToken cancellationToken = default)
        {
            if (this.cosmosContainer.Container is null)
            {
                throw new InvalidOperationException("8C7F2C03-0331-47C4-8A9A-76D9E56A4777 Unexpected Error. Container is null");
            }

            FeedIterator<T> queryIterator = this.cosmosContainer.Container.GetItemQueryIterator<T>(queryDefinition, continuationToken);
            List<T> results = new List<T>(1000);
            FeedResponse<T>? response = null;
            if (queryIterator.HasMoreResults)
            {
                response = await queryIterator.ReadNextAsync().ConfigureAwait(false);
                results.AddRange(response);
            }

            if (!string.IsNullOrWhiteSpace(response?.ContinuationToken))
            {
                CosmosQueryResults<T> paginatedResults = new CosmosQueryResults<T>()
                {
                    Results = results,
                    ResponseContinuation = response?.ContinuationToken,
                };

                return this.objectResultsProvider.Ok(paginatedResults);
            }


            return this.objectResultsProvider.Ok(results);
        }

        /// <inheritdoc/>
        public async Task<ObjectResult> UpdateItemAsync<T>(string partitionKey, T item, CancellationToken cancellationToken = default)
        {
            if (this.cosmosContainer.Container is null)
            {
                throw new InvalidOperationException("BB637DF4-BC24-470B-95A0-F4422EAA4E13 Unexpected Error. Container is null");
            }

            T result = await this.cosmosContainer.Container.UpsertItemAsync<T>(item, new PartitionKey(partitionKey)).ConfigureAwait(false);
            return this.objectResultsProvider.Ok(result);
        }
    }
}
