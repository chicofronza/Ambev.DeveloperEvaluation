namespace Ambev.DeveloperEvaluation.Domain.Interfaces
{
    /// <summary>
    /// Interface for the SaleItem entity.
    /// </summary>
    public interface ISaleItem
    {
        /// <summary>
        /// Gets the unique identifier of the sale item.
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Gets the ID of the sale this item belongs to.
        /// </summary>
        Guid SaleId { get; }

        /// <summary>
        /// Gets the ID of the product.
        /// </summary>
        Guid ProductId { get; }

        /// <summary>
        /// Gets the name of the product for denormalization purposes.
        /// </summary>
        string ProductName { get; }

        /// <summary>
        /// Gets the quantity of the product.
        /// </summary>
        int Quantity { get; }

        /// <summary>
        /// Gets the unit price of the product.
        /// </summary>
        decimal UnitPrice { get; }

        /// <summary>
        /// Gets the discount percentage applied to this item.
        /// </summary>
        decimal DiscountPercentage { get; }

        /// <summary>
        /// Gets the total amount for this item (quantity * unit price - discount).
        /// </summary>
        decimal TotalAmount { get; }

        /// <summary>
        /// Gets a value indicating whether this item is cancelled.
        /// </summary>
        bool IsCancelled { get; }

        /// <summary>
        /// Gets the date and time when the item was created.
        /// </summary>
        DateTime CreatedAt { get; }

        /// <summary>
        /// Gets the date and time of the last update to the item.
        /// </summary>
        DateTime? UpdatedAt { get; }

        /// <summary>
        /// Updates the quantity and discount of this item.
        /// </summary>
        /// <param name="quantity">The new quantity.</param>
        /// <param name="discountPercentage">The new discount percentage.</param>
        void UpdateQuantity(int quantity, decimal discountPercentage);

        /// <summary>
        /// Cancels this item.
        /// </summary>
        void Cancel();
    }
}