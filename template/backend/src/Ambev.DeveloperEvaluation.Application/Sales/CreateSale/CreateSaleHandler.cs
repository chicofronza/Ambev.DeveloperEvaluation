using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    /// <summary>
    /// Handler for processing CreateSaleCommand requests.
    /// </summary>
    public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the CreateSaleHandler class.
        /// </summary>
        /// <param name="saleRepository">The sale repository.</param>
        /// <param name="mapper">The AutoMapper instance.</param>
        public CreateSaleHandler(ISaleRepository saleRepository, IMapper mapper)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Handles the CreateSaleCommand request.
        /// </summary>
        /// <param name="command">The CreateSale command.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The created sale details.</returns>
        public async Task<CreateSaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
        {
            var validator = new CreateSaleValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            // Generate a unique sale number
            string saleNumber = GenerateSaleNumber();

            // Create the sale
            var sale = new Sale(
                saleNumber,
                command.CustomerId,
                command.CustomerName,
                command.BranchId,
                command.BranchName);

            // Add items to the sale
            foreach (var itemCommand in command.Items)
            {
                sale.AddItem(
                    itemCommand.ProductId,
                    itemCommand.ProductName,
                    itemCommand.Quantity,
                    itemCommand.UnitPrice);
            }

            // Save the sale
            var createdSale = await _saleRepository.CreateAsync(sale, cancellationToken);

            // Map to result
            return _mapper.Map<CreateSaleResult>(createdSale);
        }

        /// <summary>
        /// Generates a unique sale number.
        /// </summary>
        /// <returns>A unique sale number.</returns>
        private string GenerateSaleNumber()
        {
            // Format: SALE-{year}{month}{day}-{random6digits}
            string dateComponent = DateTime.UtcNow.ToString("yyyyMMdd");
            string randomComponent = new Random().Next(100000, 999999).ToString();
            return $"SALE-{dateComponent}-{randomComponent}";
        }
    }
}