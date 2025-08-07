using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Interfaces;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    /// <summary>
    /// Represents a sale transaction in the system.
    /// This entity follows domain-driven design principles and includes business rules validation.
    /// </summary>
    public class Sale : BaseEntity, ISale
    {
        private readonly List<SaleItem> _items = new();

        /// <summary>
        /// Gets the unique sale number.
        /// </summary>
        public string SaleNumber { get; private set; } = string.Empty;

        /// <summary>
        /// Gets the date when the sale was made.
        /// </summary>
        public DateTime SaleDate { get; private set; }

        /// <summary>
        /// Gets the customer ID associated with this sale.
        /// </summary>
        public Guid CustomerId { get; private set; }

        /// <summary>
        /// Gets the customer name for denormalization purposes.
        /// </summary>
        public string CustomerName { get; private set; } = string.Empty;

        /// <summary>
        /// Gets the branch ID where the sale was made.
        /// </summary>
        public Guid BranchId { get; private set; }

        /// <summary>
        /// Gets the branch name for denormalization purposes.
        /// </summary>
        public string BranchName { get; private set; } = string.Empty;

        /// <summary>
        /// Gets the total amount of the sale.
        /// </summary>
        public decimal TotalAmount { get; private set; }

        /// <summary>
        /// Gets the current status of the sale.
        /// </summary>
        public SaleStatus Status { get; private set; }

        /// <summary>
        /// Gets the date and time when the sale was created.
        /// </summary>
        public DateTime CreatedAt { get; private set; }

        /// <summary>
        /// Gets the date and time of the last update to the sale.
        /// </summary>
        public DateTime? UpdatedAt { get; private set; }

        /// <summary>
        /// Gets the collection of items in this sale.
        /// </summary>
        public IReadOnlyCollection<SaleItem> Items => _items.AsReadOnly();

        /// <summary>
        /// Initializes a new instance of the Sale class.
        /// </summary>
        public Sale()
        {
            CreatedAt = DateTime.UtcNow;
            Status = SaleStatus.Active;
        }

        /// <summary>
        /// Initializes a new instance of the Sale class with the specified details.
        /// </summary>
        public Sale(string saleNumber, Guid customerId, string customerName, Guid branchId, string branchName)
            : this()
        {
            SaleNumber = saleNumber;
            SaleDate = DateTime.UtcNow;
            CustomerId = customerId;
            CustomerName = customerName;
            BranchId = branchId;
            BranchName = branchName;
            TotalAmount = 0;
        }

        /// <summary>
        /// Updates the customer information for this sale.
        /// </summary>
        /// <param name="customerId">The new customer ID.</param>
        /// <param name="customerName">The new customer name.</param>
        public void UpdateCustomerInfo(Guid customerId, string customerName)
        {
            if (Status == SaleStatus.Cancelled)
            {
                throw new DomainException("Cannot update a cancelled sale");
            }

            CustomerId = customerId;
            CustomerName = customerName;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Updates the branch information for this sale.
        /// </summary>
        /// <param name="branchId">The new branch ID.</param>
        /// <param name="branchName">The new branch name.</param>
        public void UpdateBranchInfo(Guid branchId, string branchName)
        {
            if (Status == SaleStatus.Cancelled)
            {
                throw new DomainException("Cannot update a cancelled sale");
            }

            BranchId = branchId;
            BranchName = branchName;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Adds a new item to the sale.
        /// </summary>
        /// <param name="productId">The ID of the product.</param>
        /// <param name="productName">The name of the product.</param>
        /// <param name="quantity">The quantity of the product.</param>
        /// <param name="unitPrice">The unit price of the product.</param>
        /// <returns>The created sale item.</returns>
        public SaleItem AddItem(Guid productId, string productName, int quantity, decimal unitPrice)
        {
            if (Status == SaleStatus.Cancelled)
            {
                throw new DomainException("Cannot add items to a cancelled sale");
            }

            if (quantity <= 0)
            {
                throw new DomainException("Quantity must be greater than zero");
            }

            if (quantity > 20)
            {
                throw new DomainException("Cannot sell more than 20 identical items");
            }

            var existingItem = _items.FirstOrDefault(i => i.ProductId == productId);
            if (existingItem != null)
            {
                throw new DomainException("Product already exists in this sale. Please update the existing item instead.");
            }

            // Apply discount rules
            decimal discountPercentage = CalculateDiscountPercentage(quantity);

            var saleItem = new SaleItem(
                Id,
                productId,
                productName,
                quantity,
                unitPrice,
                discountPercentage);

            _items.Add(saleItem);
            RecalculateTotalAmount();
            UpdatedAt = DateTime.UtcNow;

            return saleItem;
        }

        /// <summary>
        /// Updates an existing item in the sale.
        /// </summary>
        /// <param name="productId">The ID of the product to update.</param>
        /// <param name="quantity">The new quantity of the product.</param>
        public void UpdateItem(Guid productId, int quantity)
        {
            if (Status == SaleStatus.Cancelled)
            {
                throw new DomainException("Cannot update items in a cancelled sale");
            }

            if (quantity <= 0)
            {
                throw new DomainException("Quantity must be greater than zero");
            }

            if (quantity > 20)
            {
                throw new DomainException("Cannot sell more than 20 identical items");
            }

            var existingItem = _items.FirstOrDefault(i => i.ProductId == productId);
            if (existingItem == null)
            {
                throw new DomainException("Product not found in this sale");
            }

            // Apply discount rules
            decimal discountPercentage = CalculateDiscountPercentage(quantity);

            existingItem.UpdateQuantity(quantity, discountPercentage);
            RecalculateTotalAmount();
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Cancels a specific item in the sale.
        /// </summary>
        /// <param name="productId">The ID of the product to cancel.</param>
        public void CancelItem(Guid productId)
        {
            if (Status == SaleStatus.Cancelled)
            {
                throw new DomainException("Cannot cancel items in a cancelled sale");
            }

            var existingItem = _items.FirstOrDefault(i => i.ProductId == productId);
            if (existingItem == null)
            {
                throw new DomainException("Product not found in this sale");
            }

            existingItem.Cancel();
            RecalculateTotalAmount();
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Cancels the entire sale.
        /// </summary>
        public void Cancel()
        {
            if (Status == SaleStatus.Cancelled)
            {
                throw new DomainException("Sale is already cancelled");
            }

            Status = SaleStatus.Cancelled;
            
            // Cancel all items
            foreach (var item in _items)
            {
                item.Cancel();
            }

            RecalculateTotalAmount();
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Calculates the discount percentage based on quantity according to business rules.
        /// </summary>
        /// <param name="quantity">The quantity of items.</param>
        /// <returns>The discount percentage as a decimal (0.1 = 10%).</returns>
        private decimal CalculateDiscountPercentage(int quantity)
        {
            if (quantity >= 10 && quantity <= 20)
            {
                return 0.2m; // 20% discount for 10-20 items
            }
            else if (quantity >= 4)
            {
                return 0.1m; // 10% discount for 4-9 items
            }
            
            return 0m; // No discount for less than 4 items
        }

        /// <summary>
        /// Recalculates the total amount of the sale based on all active items.
        /// </summary>
        private void RecalculateTotalAmount()
        {
            TotalAmount = _items.Where(i => !i.IsCancelled).Sum(i => i.TotalAmount);
        }

        /// <summary>
        /// Performs validation of the sale entity using the SaleValidator rules.
        /// </summary>
        /// <returns>
        /// A <see cref="ValidationResultDetail"/> containing:
        /// - IsValid: Indicates whether all validation rules passed
        /// - Errors: Collection of validation errors if any rules failed
        /// </returns>
        public ValidationResultDetail Validate()
        {
            var validator = new SaleValidator();
            var result = validator.Validate(this);
            return new ValidationResultDetail
            {
                IsValid = result.IsValid,
                Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
            };
        }
    }
}