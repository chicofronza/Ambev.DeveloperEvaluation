using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale
{
    /// <summary>
    /// Handler for processing DeleteSaleCommand requests.
    /// </summary>
    public class DeleteSaleHandler : IRequestHandler<DeleteSaleCommand, DeleteSaleResponse>
    {
        private readonly ISaleRepository _saleRepository;

        /// <summary>
        /// Initializes a new instance of the DeleteSaleHandler class.
        /// </summary>
        /// <param name="saleRepository">The sale repository.</param>
        public DeleteSaleHandler(ISaleRepository saleRepository)
        {
            _saleRepository = saleRepository;
        }

        /// <summary>
        /// Handles the DeleteSaleCommand request.
        /// </summary>
        /// <param name="request">The DeleteSale command.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The result of the delete operation.</returns>
        public async Task<DeleteSaleResponse> Handle(DeleteSaleCommand request, CancellationToken cancellationToken)
        {
            var validator = new DeleteSaleValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var sale = await _saleRepository.GetByIdAsync(request.Id, cancellationToken);
            if (sale == null)
                throw new KeyNotFoundException($"Sale with ID {request.Id} not found");

            // IMPORTANT: We follow a soft-delete approach by default, marking sales as cancelled
            // instead of physically removing them from the database. This preserves the historical
            // record and allows for better auditing and reporting.
            //
            // However, in certain scenarios (like testing, data cleanup, or regulatory requirements),
            // we may need to physically delete records. The 'canDelete' parameter provides this
            // flexibility, but should be used with caution and proper authorization.
            if (request.CanDelete)
            {
                // Physical deletion - use with caution
                bool deleted = await _saleRepository.DeleteAsync(request.Id, cancellationToken);
                if (!deleted)
                    throw new InvalidOperationException($"Failed to delete sale with ID {request.Id}");
            }
            else
            {
                // Soft deletion (default approach) - mark as cancelled
                sale.Cancel();
                await _saleRepository.UpdateAsync(sale, cancellationToken);
            }

            return new DeleteSaleResponse { Success = true };
        }
    }
}