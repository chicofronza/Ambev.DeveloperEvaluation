using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Interfaces;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    /// <summary>
    /// Represents an item within a sale.
    /// This entity follows domain-driven design principles and includes business rules validation.
    /// </summary>
    public class SaleItem : BaseEntity, ISaleItem
    {
        /// <summary>
        /// Gets the ID of the sale this item belongs to.
        /// </summary>
        public Guid SaleId { get; private set; }

        /// <summary>
        /// Gets the ID of the product.
        /// </summary>
        public Guid ProductId { get; private set; }

        /// <summary>
        /// Gets the name of the product for denormalization purposes.
        /// </summary>
        public string ProductName { get; private set; } = string.Empty;

        /// <summary>
        /// Gets the quantity of the product.
        /// </summary>
        public int Quantity { get; private set; }

        /// <summary>
        /// Gets the unit price of the product.
        /// </summary>
        public decimal UnitPrice { get; private set; }

        /// <summary>
        /// Gets the discount percentage applied to this item.
        /// </summary>
        public decimal DiscountPercentage { get; private set; }

        /// <summary>
        /// Gets the total amount for this item (quantity * unit price - discount).
        /// </summary>
        public decimal TotalAmount { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this item is cancelled.
        /// </summary>
        public bool IsCancelled { get; private set; }

        /// <summary>
        /// Gets the date and time when the item was created.
        /// </summary>
        public DateTime CreatedAt { get; private set; }

        /// <summary>
        /// Gets the date and time of the last update to the item.
        /// </summary>
        public DateTime? UpdatedAt { get; private set; }

        /// <summary>
        /// Initializes a new instance of the SaleItem class.
        /// </summary>
        public SaleItem()
        {
            CreatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Initializes a new instance of the SaleItem class with the specified details.
        /// </summary>
        public SaleItem(
            Guid saleId,
            Guid productId,
            string productName,
            int quantity,
            decimal unitPrice,
            decimal discountPercentage)
            : this()
        {
            SaleId = saleId;
            ProductId = productId;
            ProductName = productName;
            Quantity = quantity;
            UnitPrice = unitPrice;
            DiscountPercentage = discountPercentage;
            IsCancelled = false;
            CalculateTotalAmount();
        }

        /// <summary>
        /// Updates the quantity and discount of this item.
        /// </summary>
        /// <param name="quantity">The new quantity.</param>
        /// <param name="discountPercentage">The new discount percentage.</param>
        public void UpdateQuantity(int quantity, decimal discountPercentage)
        {
            if (IsCancelled)
            {
                throw new DomainException("Cannot update a cancelled item");
            }

            Quantity = quantity;
            DiscountPercentage = discountPercentage;
            CalculateTotalAmount();
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Cancels this item.
        /// </summary>
        public void Cancel()
        {
            IsCancelled = true;
            TotalAmount = 0;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Calculates the total amount for this item based on quantity, unit price, and discount.
        /// </summary>
        private void CalculateTotalAmount()
        {
            if (IsCancelled)
            {
                TotalAmount = 0;
                return;
            }

            decimal subtotal = Quantity * UnitPrice;
            decimal discountAmount = subtotal * DiscountPercentage;
            TotalAmount = subtotal - discountAmount;
        }

        /// <summary>
        /// Performs validation of the sale item entity using the SaleItemValidator rules.
        /// </summary>
        /// <returns>
        /// A <see cref="ValidationResultDetail"/> containing:
        /// - IsValid: Indicates whether all validation rules passed
        /// - Errors: Collection of validation errors if any rules failed
        /// </returns>
        public ValidationResultDetail Validate()
        {
            var validator = new SaleItemValidator();
            var result = validator.Validate(this);
            return new ValidationResultDetail
            {
                IsValid = result.IsValid,
                Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
            };
        }
    }
}