using Ambev.DeveloperEvaluation.Common.Validation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    /// <summary>
    /// Command for creating a new sale.
    /// </summary>
    /// <remarks>
    /// This command is used to capture the required data for creating a sale,
    /// including customer information, branch information, and sale items.
    /// It implements <see cref="IRequest{TResponse}"/> to initiate the request
    /// that returns a <see cref="CreateSaleResult"/>.
    /// 
    /// The data provided in this command is validated using the
    /// <see cref="CreateSaleValidator"/> which extends
    /// <see cref="AbstractValidator{T}"/> to ensure that the fields are correctly
    /// populated and follow the required rules.
    /// </remarks>
    public class CreateSaleCommand : IRequest<CreateSaleResult>
    {
        /// <summary>
        /// Gets or sets the customer ID.
        /// </summary>
        public Guid CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the customer name.
        /// </summary>
        public string CustomerName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the branch ID.
        /// </summary>
        public Guid BranchId { get; set; }

        /// <summary>
        /// Gets or sets the branch name.
        /// </summary>
        public string BranchName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the items in the sale.
        /// </summary>
        public List<CreateSaleItemCommand> Items { get; set; } = new List<CreateSaleItemCommand>();

        /// <summary>
        /// Validates the command.
        /// </summary>
        /// <returns>The validation result.</returns>
        public ValidationResultDetail Validate()
        {
            var validator = new CreateSaleValidator();
            var result = validator.Validate(this);
            return new ValidationResultDetail
            {
                IsValid = result.IsValid,
                Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
            };
        }
    }

    /// <summary>
    /// Command for creating a sale item.
    /// </summary>
    public class CreateSaleItemCommand
    {
        /// <summary>
        /// Gets or sets the product ID.
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// Gets or sets the product name.
        /// </summary>
        public string ProductName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the unit price.
        /// </summary>
        public decimal UnitPrice { get; set; }
    }
}