using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    /// <summary>
    /// Result of getting a sale.
    /// </summary>
    public class GetSaleResult
    {
        /// <summary>
        /// Gets or sets the sale ID.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the sale number.
        /// </summary>
        public string SaleNumber { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the sale date.
        /// </summary>
        public DateTime SaleDate { get; set; }

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
        /// Gets or sets the total amount of the sale.
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Gets or sets the status of the sale.
        /// </summary>
        public SaleStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the sale was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the date and time of the last update to the sale.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets the items in the sale.
        /// </summary>
        public List<SaleItemResult> Items { get; set; } = new List<SaleItemResult>();
    }

    /// <summary>
    /// Result of a sale item.
    /// </summary>
    public class SaleItemResult
    {
        /// <summary>
        /// Gets or sets the item ID.
        /// </summary>
        public Guid Id { get; set; }

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

        /// <summary>
        /// Gets or sets the discount percentage.
        /// </summary>
        public decimal DiscountPercentage { get; set; }

        /// <summary>
        /// Gets or sets the total amount.
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Gets or sets whether the item is cancelled.
        /// </summary>
        public bool IsCancelled { get; set; }
    }
}