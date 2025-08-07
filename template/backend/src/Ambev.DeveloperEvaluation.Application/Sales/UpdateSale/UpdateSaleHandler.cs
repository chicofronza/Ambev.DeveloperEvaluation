using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    /// <summary>
    /// Handler for processing UpdateSaleCommand requests.
    /// </summary>
    public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, UpdateSaleResult>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the UpdateSaleHandler class.
        /// </summary>
        /// <param name="saleRepository">The sale repository.</param>
        /// <param name="mapper">The AutoMapper instance.</param>
        public UpdateSaleHandler(ISaleRepository saleRepository, IMapper mapper)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Handles the UpdateSaleCommand request.
        /// </summary>
        /// <param name="request">The UpdateSale command.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The updated sale details.</returns>
        public async Task<UpdateSaleResult> Handle(UpdateSaleCommand request, CancellationToken cancellationToken)
        {
            var validator = new UpdateSaleValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            // Get the existing sale
            var sale = await _saleRepository.GetByIdAsync(request.Id, cancellationToken);
            if (sale == null)
                throw new KeyNotFoundException($"Sale with ID {request.Id} not found");

            // Update sale properties
            sale.UpdateCustomerInfo(request.CustomerId, request.CustomerName);
            sale.UpdateBranchInfo(request.BranchId, request.BranchName);

            // Process items
            foreach (var itemCommand in request.Items)
            {
                // Check if the item already exists in the sale
                var existingItem = sale.Items.FirstOrDefault(i => i.ProductId == itemCommand.ProductId);
                
                if (existingItem != null)
                {
                    // Update existing item
                    sale.UpdateItem(itemCommand.ProductId, itemCommand.Quantity);
                }
                else
                {
                    // Add new item
                    sale.AddItem(
                        itemCommand.ProductId,
                        itemCommand.ProductName,
                        itemCommand.Quantity,
                        itemCommand.UnitPrice);
                }
            }

            // Save changes
            var updatedSale = await _saleRepository.UpdateAsync(sale, cancellationToken);

            // Map to result
            return _mapper.Map<UpdateSaleResult>(updatedSale);
        }
    }
}