using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSales
{
    /// <summary>
    /// Response for listing sales.
    /// </summary>
    public class ListSalesResponse
    {
        /// <summary>
        /// Gets or sets the list of sales.
        /// </summary>
        public List<SaleSummaryResponse> Sales { get; set; } = new List<SaleSummaryResponse>();
    }

    /// <summary>
    /// Summary information about a sale for the response.
    /// </summary>
    public class SaleSummaryResponse
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
        /// Gets or sets the number of items in the sale.
        /// </summary>
        public int ItemCount { get; set; }
    }
}