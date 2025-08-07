namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale
{
    /// <summary>
    /// Response after creating a sale.
    /// </summary>
    public class CreateSaleResponse
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
        /// Gets or sets the items in the sale.
        /// </summary>
        public List<SaleItemResponse> Items { get; set; } = new List<SaleItemResponse>();
    }

    /// <summary>
    /// Response for a sale item.
    /// </summary>
    public class SaleItemResponse
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
    }
}