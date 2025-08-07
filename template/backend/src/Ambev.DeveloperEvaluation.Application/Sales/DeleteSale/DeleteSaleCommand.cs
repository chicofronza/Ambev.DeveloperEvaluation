using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale
{
    /// <summary>
    /// Command for deleting a sale.
    /// </summary>
    public record DeleteSaleCommand : IRequest<DeleteSaleResponse>
    {
        /// <summary>
        /// The unique identifier of the sale to delete.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Gets a value indicating whether the sale should be physically deleted from the database.
        /// When false (default), the sale is marked as cancelled instead of being physically deleted.
        /// </summary>
        public bool CanDelete { get; }

        /// <summary>
        /// Initializes a new instance of DeleteSaleCommand.
        /// </summary>
        /// <param name="id">The ID of the sale to delete.</param>
        /// <param name="canDelete">Whether the sale should be physically deleted.</param>
        public DeleteSaleCommand(Guid id, bool canDelete = false)
        {
            Id = id;
            CanDelete = canDelete;
        }
    }
}