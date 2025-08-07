using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    /// <summary>
    /// Validator for the UpdateSaleCommand.
    /// </summary>
    public class UpdateSaleValidator : AbstractValidator<UpdateSaleCommand>
    {
        /// <summary>
        /// Initializes a new instance of the UpdateSaleValidator class.
        /// </summary>
        public UpdateSaleValidator()
        {
            RuleFor(c => c.Id)
                .NotEmpty().WithMessage("Sale ID is required.");

            RuleFor(c => c.CustomerId)
                .NotEmpty().WithMessage("Customer ID is required.");

            RuleFor(c => c.CustomerName)
                .NotEmpty().WithMessage("Customer name is required.")
                .MaximumLength(100).WithMessage("Customer name cannot exceed 100 characters.");

            RuleFor(c => c.BranchId)
                .NotEmpty().WithMessage("Branch ID is required.");

            RuleFor(c => c.BranchName)
                .NotEmpty().WithMessage("Branch name is required.")
                .MaximumLength(100).WithMessage("Branch name cannot exceed 100 characters.");

            RuleFor(c => c.Items)
                .NotEmpty().WithMessage("At least one item is required.");

            RuleForEach(c => c.Items).SetValidator(new UpdateSaleItemValidator());
        }
    }

    /// <summary>
    /// Validator for the UpdateSaleItemCommand.
    /// </summary>
    public class UpdateSaleItemValidator : AbstractValidator<UpdateSaleItemCommand>
    {
        /// <summary>
        /// Initializes a new instance of the UpdateSaleItemValidator class.
        /// </summary>
        public UpdateSaleItemValidator()
        {
            RuleFor(i => i.ProductId)
                .NotEmpty().WithMessage("Product ID is required.");

            RuleFor(i => i.ProductName)
                .NotEmpty().WithMessage("Product name is required.")
                .MaximumLength(100).WithMessage("Product name cannot exceed 100 characters.");

            RuleFor(i => i.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.")
                .LessThanOrEqualTo(20).WithMessage("Cannot sell more than 20 identical items.");

            RuleFor(i => i.UnitPrice)
                .GreaterThan(0).WithMessage("Unit price must be greater than zero.");
        }
    }
}