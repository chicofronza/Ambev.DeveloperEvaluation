using System.ComponentModel.DataAnnotations;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale
{
    /// <summary>
    /// Request to create a sale.
    /// </summary>
    public class CreateSaleRequest
    {
        /// <summary>
        /// Gets or sets the customer ID.
        /// </summary>
        [Required]
        public Guid CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the customer name.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string CustomerName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the branch ID.
        /// </summary>
        [Required]
        public Guid BranchId { get; set; }

        /// <summary>
        /// Gets or sets the branch name.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string BranchName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the items in the sale.
        /// </summary>
        [Required]
        [MinLength(1, ErrorMessage = "At least one item is required")]
        public List<CreateSaleItemRequest> Items { get; set; } = new List<CreateSaleItemRequest>();
    }

    /// <summary>
    /// Request to create a sale item.
    /// </summary>
    public class CreateSaleItemRequest
    {
        /// <summary>
        /// Gets or sets the product ID.
        /// </summary>
        [Required]
        public Guid ProductId { get; set; }

        /// <summary>
        /// Gets or sets the product name.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string ProductName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        [Required]
        [Range(1, 20, ErrorMessage = "Quantity must be between 1 and 20")]
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the unit price.
        /// </summary>
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Unit price must be greater than zero")]
        public decimal UnitPrice { get; set; }
    }
}