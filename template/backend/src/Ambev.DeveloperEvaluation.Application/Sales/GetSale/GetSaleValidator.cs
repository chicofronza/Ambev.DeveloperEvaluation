using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    /// <summary>
    /// Validator for the GetSaleCommand.
    /// </summary>
    public class GetSaleValidator : AbstractValidator<GetSaleCommand>
    {
        /// <summary>
        /// Initializes a new instance of the GetSaleValidator class.
        /// </summary>
        public GetSaleValidator()
        {
            RuleFor(c => c.Id)
                .NotEmpty().WithMessage("Sale ID is required.");
        }
    }
}