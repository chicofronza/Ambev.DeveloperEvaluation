using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSales
{
    /// <summary>
    /// Validator for ListSalesRequest.
    /// </summary>
    public class ListSalesRequestValidator : AbstractValidator<ListSalesRequest>
    {
        /// <summary>
        /// Initializes validation rules for ListSalesRequest.
        /// </summary>
        public ListSalesRequestValidator()
        {
            // Validate that if both dates are provided, start date is not after end date
            When(r => r.StartDate.HasValue && r.EndDate.HasValue, () =>
            {
                RuleFor(r => r.StartDate)
                    .LessThanOrEqualTo(r => r.EndDate)
                    .WithMessage("Start date must be before or equal to end date.");
            });
        }
    }
}