using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale
{
    /// <summary>
    /// Validator for the DeleteSaleCommand.
    /// </summary>
    public class DeleteSaleValidator : AbstractValidator<DeleteSaleCommand>
    {
        /// <summary>
        /// Initializes a new instance of the DeleteSaleValidator class.
        /// </summary>
        public DeleteSaleValidator()
        {
            RuleFor(c => c.Id)
                .NotEmpty().WithMessage("Sale ID is required.");
        }
    }
}