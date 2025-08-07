using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation
{
    /// <summary>
    /// Validator for the SaleItem entity.
    /// </summary>
    public class SaleItemValidator : AbstractValidator<SaleItem>
    {
        /// <summary>
        /// Initializes a new instance of the SaleItemValidator class.
        /// </summary>
        public SaleItemValidator()
        {
            RuleFor(si => si.SaleId)
                .NotEmpty().WithMessage("Sale ID is required.");

            RuleFor(si => si.ProductId)
                .NotEmpty().WithMessage("Product ID is required.");

            RuleFor(si => si.ProductName)
                .NotEmpty().WithMessage("Product name is required.")
                .MaximumLength(100).WithMessage("Product name cannot exceed 100 characters.");

            RuleFor(si => si.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.")
                .LessThanOrEqualTo(20).WithMessage("Cannot sell more than 20 identical items.");

            RuleFor(si => si.UnitPrice)
                .GreaterThan(0).WithMessage("Unit price must be greater than zero.");

            RuleFor(si => si.DiscountPercentage)
                .GreaterThanOrEqualTo(0).WithMessage("Discount percentage cannot be negative.")
                .LessThanOrEqualTo(1).WithMessage("Discount percentage cannot exceed 100%.");

            RuleFor(si => si.TotalAmount)
                .GreaterThanOrEqualTo(0).WithMessage("Total amount cannot be negative.");

            // Business rule: Purchases below 4 items cannot have a discount
            RuleFor(si => si.DiscountPercentage)
                .Equal(0).When(si => si.Quantity < 4)
                .WithMessage("Purchases below 4 items cannot have a discount.");

            // Business rule: Purchases between 4 and 9 items have a 10% discount
            RuleFor(si => si.DiscountPercentage)
                .Equal(0.1m).When(si => si.Quantity >= 4 && si.Quantity < 10)
                .WithMessage("Purchases between 4 and 9 items should have a 10% discount.");

            // Business rule: Purchases between 10 and 20 items have a 20% discount
            RuleFor(si => si.DiscountPercentage)
                .Equal(0.2m).When(si => si.Quantity >= 10 && si.Quantity <= 20)
                .WithMessage("Purchases between 10 and 20 items should have a 20% discount.");
        }
    }
}