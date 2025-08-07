using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation
{
    /// <summary>
    /// Validator for the Sale entity.
    /// </summary>
    public class SaleValidator : AbstractValidator<Sale>
    {
        /// <summary>
        /// Initializes a new instance of the SaleValidator class.
        /// </summary>
        public SaleValidator()
        {
            RuleFor(s => s.SaleNumber)
                .NotEmpty().WithMessage("Sale number is required.")
                .MaximumLength(50).WithMessage("Sale number cannot exceed 50 characters.");

            RuleFor(s => s.SaleDate)
                .NotEmpty().WithMessage("Sale date is required.");

            RuleFor(s => s.CustomerId)
                .NotEmpty().WithMessage("Customer ID is required.");

            RuleFor(s => s.CustomerName)
                .NotEmpty().WithMessage("Customer name is required.")
                .MaximumLength(100).WithMessage("Customer name cannot exceed 100 characters.");

            RuleFor(s => s.BranchId)
                .NotEmpty().WithMessage("Branch ID is required.");

            RuleFor(s => s.BranchName)
                .NotEmpty().WithMessage("Branch name is required.")
                .MaximumLength(100).WithMessage("Branch name cannot exceed 100 characters.");

            RuleFor(s => s.TotalAmount)
                .GreaterThanOrEqualTo(0).WithMessage("Total amount cannot be negative.");

            RuleFor(s => s.Status)
                .IsInEnum().WithMessage("Invalid sale status.");

            RuleFor(s => s.Items)
                .NotEmpty().WithMessage("Sale must have at least one item.");

            RuleForEach(s => s.Items)
                .SetValidator(new SaleItemValidator());
        }
    }
}