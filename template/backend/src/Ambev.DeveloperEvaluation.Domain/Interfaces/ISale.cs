using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Domain.Interfaces
{
    /// <summary>
    /// Interface for the Sale entity.
    /// </summary>
    public interface ISale
    {
        /// <summary>
        /// Gets the unique identifier of the sale.
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Gets the unique sale number.
        /// </summary>
        string SaleNumber { get; }

        /// <summary>
        /// Gets the date when the sale was made.
        /// </summary>
        DateTime SaleDate { get; }

        /// <summary>
        /// Gets the customer ID associated with this sale.
        /// </summary>
        Guid CustomerId { get; }

        /// <summary>
        /// Gets the customer name for denormalization purposes.
        /// </summary>
        string CustomerName { get; }

        /// <summary>
        /// Gets the branch ID where the sale was made.
        /// </summary>
        Guid BranchId { get; }

        /// <summary>
        /// Gets the branch name for denormalization purposes.
        /// </summary>
        string BranchName { get; }

        /// <summary>
        /// Gets the total amount of the sale.
        /// </summary>
        decimal TotalAmount { get; }

        /// <summary>
        /// Gets the current status of the sale.
        /// </summary>
        SaleStatus Status { get; }

        /// <summary>
        /// Gets the date and time when the sale was created.
        /// </summary>
        DateTime CreatedAt { get; }

        /// <summary>
        /// Gets the date and time of the last update to the sale.
        /// </summary>
        DateTime? UpdatedAt { get; }

        /// <summary>
        /// Gets the collection of items in this sale.
        /// </summary>
        IReadOnlyCollection<SaleItem> Items { get; }

        /// <summary>
        /// Updates the customer information for this sale.
        /// </summary>
        /// <param name="customerId">The new customer ID.</param>
        /// <param name="customerName">The new customer name.</param>
        void UpdateCustomerInfo(Guid customerId, string customerName);

        /// <summary>
        /// Updates the branch information for this sale.
        /// </summary>
        /// <param name="branchId">The new branch ID.</param>
        /// <param name="branchName">The new branch name.</param>
        void UpdateBranchInfo(Guid branchId, string branchName);

        /// <summary>
        /// Adds a new item to the sale.
        /// </summary>
        /// <param name="productId">The ID of the product.</param>
        /// <param name="productName">The name of the product.</param>
        /// <param name="quantity">The quantity of the product.</param>
        /// <param name="unitPrice">The unit price of the product.</param>
        /// <returns>The created sale item.</returns>
        SaleItem AddItem(Guid productId, string productName, int quantity, decimal unitPrice);

        /// <summary>
        /// Updates an existing item in the sale.
        /// </summary>
        /// <param name="productId">The ID of the product to update.</param>
        /// <param name="quantity">The new quantity of the product.</param>
        void UpdateItem(Guid productId, int quantity);

        /// <summary>
        /// Cancels a specific item in the sale.
        /// </summary>
        /// <param name="productId">The ID of the product to cancel.</param>
        void CancelItem(Guid productId);

        /// <summary>
        /// Cancels the entire sale.
        /// </summary>
        void Cancel();
    }
}