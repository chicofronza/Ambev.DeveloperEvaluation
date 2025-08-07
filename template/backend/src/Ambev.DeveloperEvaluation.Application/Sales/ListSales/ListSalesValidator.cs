using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales
{
    /// <summary>
    /// Validator for the ListSalesCommand.
    /// </summary>
    public class ListSalesValidator : AbstractValidator<ListSalesCommand>
    {
        /// <summary>
        /// Initializes a new instance of the ListSalesValidator class.
        /// </summary>
        public ListSalesValidator()
        {
            // Validate that if both dates are provided, start date is not after end date
            When(c => c.StartDate.HasValue && c.EndDate.HasValue, () =>
            {
                RuleFor(c => c.StartDate)
                    .LessThanOrEqualTo(c => c.EndDate)
                    .WithMessage("Start date must be before or equal to end date.");
            });
        }
    }
}