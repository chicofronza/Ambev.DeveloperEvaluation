using MediatR;
using System.Collections.Generic;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    /// <summary>
    /// Command for updating a sale.
    /// </summary>
    public class UpdateSaleCommand : IRequest<UpdateSaleResult>
    {
        /// <summary>
        /// Gets or sets the sale ID.
        /// </summary>
        public Guid Id { get; set; }

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
        /// Gets or sets the items to update.
        /// </summary>
        public List<UpdateSaleItemCommand> Items { get; set; } = new List<UpdateSaleItemCommand>();
    }

    /// <summary>
    /// Command for updating a sale item.
    /// </summary>
    public class UpdateSaleItemCommand
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