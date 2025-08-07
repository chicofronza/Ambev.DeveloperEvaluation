using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales
{
    /// <summary>
    /// Handler for processing ListSalesCommand requests.
    /// </summary>
    public class ListSalesHandler : IRequestHandler<ListSalesCommand, ListSalesResult>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the ListSalesHandler class.
        /// </summary>
        /// <param name="saleRepository">The sale repository.</param>
        /// <param name="mapper">The AutoMapper instance.</param>
        public ListSalesHandler(ISaleRepository saleRepository, IMapper mapper)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Handles the ListSalesCommand request.
        /// </summary>
        /// <param name="request">The ListSales command.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The list of sales matching the criteria.</returns>
        public async Task<ListSalesResult> Handle(ListSalesCommand request, CancellationToken cancellationToken)
        {
            var validator = new ListSalesValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var sales = await _saleRepository.ListAsync(
                request.CustomerId,
                request.BranchId,
                request.StartDate,
                request.EndDate,
                cancellationToken);

            var result = new ListSalesResult
            {
                Sales = sales.Select(s => new SaleSummary
                {
                    Id = s.Id,
                    SaleNumber = s.SaleNumber,
                    SaleDate = s.SaleDate,
                    CustomerId = s.CustomerId,
                    CustomerName = s.CustomerName,
                    BranchId = s.BranchId,
                    BranchName = s.BranchName,
                    TotalAmount = s.TotalAmount,
                    Status = s.Status,
                    ItemCount = s.Items.Count(i => !i.IsCancelled)
                }).ToList()
            };

            return result;
        }
    }
}