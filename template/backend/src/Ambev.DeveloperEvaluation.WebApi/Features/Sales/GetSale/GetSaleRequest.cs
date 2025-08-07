using System.ComponentModel.DataAnnotations;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale
{
    /// <summary>
    /// Request to get a sale by ID.
    /// </summary>
    public class GetSaleRequest
    {
        /// <summary>
        /// Gets or sets the sale ID.
        /// </summary>
        [Required]
        public Guid Id { get; set; }
    }
}