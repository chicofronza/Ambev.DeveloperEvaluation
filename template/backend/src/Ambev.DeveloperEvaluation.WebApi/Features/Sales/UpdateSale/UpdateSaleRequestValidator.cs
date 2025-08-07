using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale
{
    /// <summary>
    /// Validator for the UpdateSaleRequest.
    /// </summary>
    public class UpdateSaleRequestValidator : AbstractValidator<UpdateSaleRequest>
    {
        /// <summary>
        /// Initializes a new instance of the UpdateSaleRequestValidator class.
        /// </summary>
        public UpdateSaleRequestValidator()
        {
            RuleFor(r => r.CustomerId)
                .NotEmpty().WithMessage("Customer ID is required.");

            RuleFor(r => r.CustomerName)
                .NotEmpty().WithMessage("Customer name is required.")
                .MaximumLength(100).WithMessage("Customer name cannot exceed 100 characters.");

            RuleFor(r => r.BranchId)
                .NotEmpty().WithMessage("Branch ID is required.");

            RuleFor(r => r.BranchName)
                .NotEmpty().WithMessage("Branch name is required.")
                .MaximumLength(100).WithMessage("Branch name cannot exceed 100 characters.");

            RuleFor(r => r.Items)
                .NotEmpty().WithMessage("At least one item is required.");

            RuleForEach(r => r.Items).SetValidator(new UpdateSaleItemRequestValidator());
        }
    }

    /// <summary>
    /// Validator for the UpdateSaleItemRequest.
    /// </summary>
    public class UpdateSaleItemRequestValidator : AbstractValidator<UpdateSaleItemRequest>
    {
        /// <summary>
        /// Initializes a new instance of the UpdateSaleItemRequestValidator class.
        /// </summary>
        public UpdateSaleItemRequestValidator()
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