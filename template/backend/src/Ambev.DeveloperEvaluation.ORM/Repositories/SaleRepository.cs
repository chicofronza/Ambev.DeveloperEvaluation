using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    /// <summary>
    /// Implementation of the ISaleRepository interface.
    /// </summary>
    public class SaleRepository : ISaleRepository
    {
        private readonly DefaultContext _context;

        /// <summary>
        /// Initializes a new instance of the SaleRepository class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public SaleRepository(DefaultContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates a new sale in the repository.
        /// </summary>
        /// <param name="sale">The sale to create.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The created sale.</returns>
        public async Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken = default)
        {
            await _context.Sales.AddAsync(sale, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return sale;
        }

        /// <summary>
        /// Retrieves a sale by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the sale.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The sale if found, null otherwise.</returns>
        public async Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Sales
                .Include(s => s.Items)
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        }

        /// <summary>
        /// Retrieves a sale by its sale number.
        /// </summary>
        /// <param name="saleNumber">The sale number to search for.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The sale if found, null otherwise.</returns>
        public async Task<Sale?> GetBySaleNumberAsync(string saleNumber, CancellationToken cancellationToken = default)
        {
            return await _context.Sales
                .Include(s => s.Items)
                .FirstOrDefaultAsync(s => s.SaleNumber == saleNumber, cancellationToken);
        }

        /// <summary>
        /// Updates an existing sale in the repository.
        /// </summary>
        /// <param name="sale">The sale to update.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The updated sale.</returns>
        public async Task<Sale> UpdateAsync(Sale sale, CancellationToken cancellationToken = default)
        {
            _context.Sales.Update(sale);
            await _context.SaveChangesAsync(cancellationToken);
            return sale;
        }

        /// <summary>
        /// Physically deletes a sale from the repository.
        /// </summary>
        /// <param name="id">The unique identifier of the sale to delete.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>True if the sale was deleted, false if not found.</returns>
        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var sale = await _context.Sales.FindAsync(new object[] { id }, cancellationToken);
            if (sale == null)
                return false;

            _context.Sales.Remove(sale);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        /// <summary>
        /// Lists all sales with optional filtering.
        /// </summary>
        /// <param name="customerId">Optional customer ID to filter by.</param>
        /// <param name="branchId">Optional branch ID to filter by.</param>
        /// <param name="startDate">Optional start date to filter by.</param>
        /// <param name="endDate">Optional end date to filter by.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A list of sales matching the criteria.</returns>
        public async Task<IEnumerable<Sale>> ListAsync(
            Guid? customerId = null,
            Guid? branchId = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            CancellationToken cancellationToken = default)
        {
            IQueryable<Sale> query = _context.Sales.Include(s => s.Items);

            if (customerId.HasValue)
            {
                query = query.Where(s => s.CustomerId == customerId.Value);
            }

            if (branchId.HasValue)
            {
                query = query.Where(s => s.BranchId == branchId.Value);
            }

            if (startDate.HasValue)
            {
                query = query.Where(s => s.SaleDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(s => s.SaleDate <= endDate.Value);
            }

            return await query.ToListAsync(cancellationToken);
        }
    }
}