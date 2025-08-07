using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories
{
    /// <summary>
    /// Repository interface for Sale entity operations.
    /// </summary>
    public interface ISaleRepository
    {
        /// <summary>
        /// Creates a new sale in the repository.
        /// </summary>
        /// <param name="sale">The sale to create.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The created sale.</returns>
        Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a sale by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the sale.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The sale if found, null otherwise.</returns>
        Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a sale by its sale number.
        /// </summary>
        /// <param name="saleNumber">The sale number to search for.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The sale if found, null otherwise.</returns>
        Task<Sale?> GetBySaleNumberAsync(string saleNumber, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing sale in the repository.
        /// </summary>
        /// <param name="sale">The sale to update.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The updated sale.</returns>
        Task<Sale> UpdateAsync(Sale sale, CancellationToken cancellationToken = default);

        /// <summary>
        /// Physically deletes a sale from the repository.
        /// </summary>
        /// <param name="id">The unique identifier of the sale to delete.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>True if the sale was deleted, false if not found.</returns>
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Lists all sales with optional filtering.
        /// </summary>
        /// <param name="customerId">Optional customer ID to filter by.</param>
        /// <param name="branchId">Optional branch ID to filter by.</param>
        /// <param name="startDate">Optional start date to filter by.</param>
        /// <param name="endDate">Optional end date to filter by.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A list of sales matching the criteria.</returns>
        Task<IEnumerable<Sale>> ListAsync(
            Guid? customerId = null,
            Guid? branchId = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            CancellationToken cancellationToken = default);
    }
}