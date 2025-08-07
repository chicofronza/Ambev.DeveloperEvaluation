namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.DeleteSale
{
    /// <summary>
    /// Request model for deleting a sale.
    /// </summary>
    public class DeleteSaleRequest
    {
        /// <summary>
        /// The unique identifier of the sale to delete.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the sale should be physically deleted from the database.
        /// When false (default), the sale is marked as cancelled instead of being physically deleted.
        /// </summary>
        public bool CanDelete { get; set; } = false;
    }
}