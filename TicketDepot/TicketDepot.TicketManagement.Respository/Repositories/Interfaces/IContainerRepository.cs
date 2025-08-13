using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;

namespace TicketDepot.TicketManagement.Repository
{
    public interface IContainerRepository<TR>
        where TR : ICosmosContainer
    {
        /// <summary>
        /// Adds an document to the current container.
        /// </summary>
        /// <typeparam name="T">The type of document, based on the current container.</typeparam>
        /// <param name="item">The instance of the document to add.</param>
        /// <returns>The added document.</returns>
        Task<ObjectResult> AddItemAsync<T>(T item, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a document from the current container.
        /// </summary>
        /// <typeparam name="T">The type of object to update.</typeparam>
        /// <param name="id">The Id of the document.</param>
        /// <param name="partitionKey">The Partition key to use.</param>
        /// <returns>a <see cref="Task"/>.</returns>
        Task<ObjectResult> DeleteItemAsync<T>(string id, string partitionKey, CancellationToken cancellationToken = default);

        /// <summary>
        /// Generic version that specifies the
        /// return type.
        /// </summary>
        /// <typeparam name="T">The type of the object being retrieved.</typeparam>
        /// <param name="queryDefinition">The <see cref="QueryDefinition"/> to execute.</param>
        /// <param name="continuationToken">The continuation token, when specified use to get the next batch.</param>
        /// <returns>A list of the objects of the specified return type.</returns>
        Task<ObjectResult> GetItemsAsync<T>(QueryDefinition queryDefinition, string? continuationToken = null,  CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a single document base on the Id and partition key.
        /// </summary>
        /// <typeparam name="T">The type of the object being retrieved.</typeparam>
        /// <param name="id">The Id of the document.</param>
        /// <param name="partitionKey">The Partition Key to use.</param>
        /// <returns>An object based on the current container.</returns>
        Task<ObjectResult> GetItemAsync<T>(string id, string partitionKey, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates a document for the current container.
        /// </summary>
        /// <typeparam name="T">The type of object to update.</typeparam>
        /// <param name="partitionKey">The Partition key to use.</param>
        /// <param name="item">The instance of the document.</param>
        /// <returns>a <see cref="Task"/>.</returns>
        Task<ObjectResult> UpdateItemAsync<T>(string partitionKey, T item, CancellationToken cancellationToken = default);
    }
}
