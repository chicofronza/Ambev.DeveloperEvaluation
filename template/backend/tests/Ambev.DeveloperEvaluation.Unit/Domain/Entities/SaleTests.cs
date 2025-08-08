using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Contains unit tests for the Sale entity class.
/// Tests cover business rules, status changes, item management, and validation scenarios.
/// </summary>
public class SaleTests
{
    /// <summary>
    /// Tests that a new sale is created with the correct initial values.
    /// </summary>
    [Fact(DisplayName = "New sale should have correct initial values")]
    public void Given_NewSale_When_Created_Then_ShouldHaveCorrectInitialValues()
    {
        // Arrange
        var saleNumber = SaleTestData.GenerateValidSaleNumber();
        var customerId = Guid.NewGuid();
        var customerName = SaleTestData.GenerateValidCustomerName();
        var branchId = Guid.NewGuid();
        var branchName = SaleTestData.GenerateValidBranchName();

        // Act
        var sale = new Sale(saleNumber, customerId, customerName, branchId, branchName);

        // Assert
        sale.SaleNumber.Should().Be(saleNumber);
        sale.CustomerId.Should().Be(customerId);
        sale.CustomerName.Should().Be(customerName);
        sale.BranchId.Should().Be(branchId);
        sale.BranchName.Should().Be(branchName);
        sale.Status.Should().Be(SaleStatus.Active);
        sale.TotalAmount.Should().Be(0);
        sale.Items.Should().BeEmpty();
    }

    /// <summary>
    /// Tests that when a sale is cancelled, its status changes to Cancelled.
    /// </summary>
    [Fact(DisplayName = "Sale status should change to Cancelled when cancelled")]
    public void Given_ActiveSale_When_Cancelled_Then_StatusShouldBeCancelled()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSaleWithItems(1);

        // Act
        sale.Cancel();

        // Assert
        sale.Status.Should().Be(SaleStatus.Cancelled);
    }

    /// <summary>
    /// Tests that when a sale with items is cancelled, all items are cancelled.
    /// </summary>
    [Fact(DisplayName = "All items should be cancelled when sale is cancelled")]
    public void Given_SaleWithItems_When_Cancelled_Then_AllItemsShouldBeCancelled()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSaleWithItems(3);

        // Act
        sale.Cancel();

        // Assert
        sale.Status.Should().Be(SaleStatus.Cancelled);
        sale.Items.Should().AllSatisfy(item => item.IsCancelled.Should().BeTrue());
    }

    /// <summary>
    /// Tests that attempting to cancel an already cancelled sale throws an exception.
    /// </summary>
    [Fact(DisplayName = "Cancelling an already cancelled sale should throw exception")]
    public void Given_CancelledSale_When_Cancelled_Then_ShouldThrowException()
    {
        // Arrange
        var sale = SaleTestData.GenerateCancelledSale();

        // Act & Assert
        Assert.Throws<DomainException>(() => sale.Cancel())
            .Message.Should().Be("Sale is already cancelled");
    }

    /// <summary>
    /// Tests that adding an item to a sale increases the item count and updates the total amount.
    /// </summary>
    [Fact(DisplayName = "Adding item should increase item count and update total amount")]
    public void Given_Sale_When_ItemAdded_Then_ShouldIncreaseItemCountAndUpdateTotalAmount()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        var productId = Guid.NewGuid();
        var productName = "Test Product";
        var quantity = 2;
        var unitPrice = 10.0m;
        var expectedItemCount = 1;
        var expectedTotalAmount = quantity * unitPrice; // No discount for quantity < 4

        // Act
        var item = sale.AddItem(productId, productName, quantity, unitPrice);

        // Assert
        sale.Items.Should().HaveCount(expectedItemCount);
        sale.TotalAmount.Should().Be(expectedTotalAmount);
        item.ProductId.Should().Be(productId);
        item.ProductName.Should().Be(productName);
        item.Quantity.Should().Be(quantity);
        item.UnitPrice.Should().Be(unitPrice);
        item.DiscountPercentage.Should().Be(0); // No discount for quantity < 4
        item.TotalAmount.Should().Be(expectedTotalAmount);
    }

    /// <summary>
    /// Tests that adding an item with a quantity that qualifies for a 10% discount applies the correct discount.
    /// </summary>
    [Fact(DisplayName = "Adding item with quantity 4-9 should apply 10% discount")]
    public void Given_Sale_When_ItemAddedWithQuantity4To9_Then_ShouldApply10PercentDiscount()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        var productId = Guid.NewGuid();
        var productName = "Test Product";
        var quantity = 5;
        var unitPrice = 10.0m;
        var discountPercentage = 0.1m; // 10% discount for quantity 4-9
        var expectedTotalAmount = quantity * unitPrice * (1 - discountPercentage);

        // Act
        var item = sale.AddItem(productId, productName, quantity, unitPrice);

        // Assert
        item.DiscountPercentage.Should().Be(discountPercentage);
        item.TotalAmount.Should().Be(expectedTotalAmount);
        sale.TotalAmount.Should().Be(expectedTotalAmount);
    }

    /// <summary>
    /// Tests that adding an item with a quantity that qualifies for a 20% discount applies the correct discount.
    /// </summary>
    [Fact(DisplayName = "Adding item with quantity 10-20 should apply 20% discount")]
    public void Given_Sale_When_ItemAddedWithQuantity10To20_Then_ShouldApply20PercentDiscount()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        var productId = Guid.NewGuid();
        var productName = "Test Product";
        var quantity = 15;
        var unitPrice = 10.0m;
        var discountPercentage = 0.2m; // 20% discount for quantity 10-20
        var expectedTotalAmount = quantity * unitPrice * (1 - discountPercentage);

        // Act
        var item = sale.AddItem(productId, productName, quantity, unitPrice);

        // Assert
        item.DiscountPercentage.Should().Be(discountPercentage);
        item.TotalAmount.Should().Be(expectedTotalAmount);
        sale.TotalAmount.Should().Be(expectedTotalAmount);
    }

    /// <summary>
    /// Tests that adding an item with a quantity greater than 20 throws an exception.
    /// </summary>
    [Fact(DisplayName = "Adding item with quantity > 20 should throw exception")]
    public void Given_Sale_When_ItemAddedWithQuantityGreaterThan20_Then_ShouldThrowException()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        var productId = Guid.NewGuid();
        var productName = "Test Product";
        var quantity = 21;
        var unitPrice = 10.0m;

        // Act & Assert
        Assert.Throws<DomainException>(() => sale.AddItem(productId, productName, quantity, unitPrice))
            .Message.Should().Be("Cannot sell more than 20 identical items");
    }

    /// <summary>
    /// Tests that adding an item with a quantity less than or equal to 0 throws an exception.
    /// </summary>
    [Fact(DisplayName = "Adding item with quantity <= 0 should throw exception")]
    public void Given_Sale_When_ItemAddedWithQuantityLessThanOrEqualToZero_Then_ShouldThrowException()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        var productId = Guid.NewGuid();
        var productName = "Test Product";
        var quantity = 0;
        var unitPrice = 10.0m;

        // Act & Assert
        Assert.Throws<DomainException>(() => sale.AddItem(productId, productName, quantity, unitPrice))
            .Message.Should().Be("Quantity must be greater than zero");
    }

    /// <summary>
    /// Tests that adding an item with a product ID that already exists in the sale throws an exception.
    /// </summary>
    [Fact(DisplayName = "Adding item with duplicate product ID should throw exception")]
    public void Given_SaleWithItem_When_ItemAddedWithDuplicateProductId_Then_ShouldThrowException()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        var productId = Guid.NewGuid();
        var productName = "Test Product";
        var quantity = 1;
        var unitPrice = 10.0m;

        sale.AddItem(productId, productName, quantity, unitPrice);

        // Act & Assert
        Assert.Throws<DomainException>(() => sale.AddItem(productId, "Another Product", 2, 20.0m))
            .Message.Should().Be("Product already exists in this sale. Please update the existing item instead.");
    }

    /// <summary>
    /// Tests that adding an item to a cancelled sale throws an exception.
    /// </summary>
    [Fact(DisplayName = "Adding item to cancelled sale should throw exception")]
    public void Given_CancelledSale_When_ItemAdded_Then_ShouldThrowException()
    {
        // Arrange
        var sale = SaleTestData.GenerateCancelledSale();
        var productId = Guid.NewGuid();
        var productName = "Test Product";
        var quantity = 1;
        var unitPrice = 10.0m;

        // Act & Assert
        Assert.Throws<DomainException>(() => sale.AddItem(productId, productName, quantity, unitPrice))
            .Message.Should().Be("Cannot add items to a cancelled sale");
    }

    /// <summary>
    /// Tests that updating an item's quantity updates the item and the sale's total amount.
    /// </summary>
    [Fact(DisplayName = "Updating item quantity should update item and total amount")]
    public void Given_SaleWithItem_When_ItemQuantityUpdated_Then_ShouldUpdateItemAndTotalAmount()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        var productId = Guid.NewGuid();
        var productName = "Test Product";
        var initialQuantity = 2;
        var newQuantity = 5;
        var unitPrice = 10.0m;
        var newDiscountPercentage = 0.1m; // 10% discount for quantity 4-9
        var newTotalAmount = newQuantity * unitPrice * (1 - newDiscountPercentage);

        sale.AddItem(productId, productName, initialQuantity, unitPrice);
        
        // Act
        sale.UpdateItem(productId, newQuantity);

        // Assert
        var updatedItem = sale.Items.First(i => i.ProductId == productId);
        updatedItem.Quantity.Should().Be(newQuantity);
        updatedItem.DiscountPercentage.Should().Be(newDiscountPercentage);
        updatedItem.TotalAmount.Should().Be(newTotalAmount);
        sale.TotalAmount.Should().Be(newTotalAmount);
    }

    /// <summary>
    /// Tests that updating an item in a cancelled sale throws an exception.
    /// </summary>
    [Fact(DisplayName = "Updating item in cancelled sale should throw exception")]
    public void Given_CancelledSale_When_ItemUpdated_Then_ShouldThrowException()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        var productId = Guid.NewGuid();
        sale.AddItem(productId, "Test Product", 1, 10.0m);
        sale.Cancel();

        // Act & Assert
        Assert.Throws<DomainException>(() => sale.UpdateItem(productId, 2))
            .Message.Should().Be("Cannot update items in a cancelled sale");
    }

    /// <summary>
    /// Tests that updating a non-existent item throws an exception.
    /// </summary>
    [Fact(DisplayName = "Updating non-existent item should throw exception")]
    public void Given_Sale_When_NonExistentItemUpdated_Then_ShouldThrowException()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        var nonExistentProductId = Guid.NewGuid();

        // Act & Assert
        Assert.Throws<DomainException>(() => sale.UpdateItem(nonExistentProductId, 2))
            .Message.Should().Be("Product not found in this sale");
    }

    /// <summary>
    /// Tests that cancelling an item updates the item's status and the sale's total amount.
    /// </summary>
    [Fact(DisplayName = "Cancelling item should update item status and total amount")]
    public void Given_SaleWithItem_When_ItemCancelled_Then_ShouldUpdateItemStatusAndTotalAmount()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        var productId1 = Guid.NewGuid();
        var productId2 = Guid.NewGuid();
        sale.AddItem(productId1, "Product 1", 2, 10.0m); // Total: 20
        sale.AddItem(productId2, "Product 2", 3, 15.0m); // Total: 45
        var expectedTotalAmount = 45.0m; // Only Product 2 remains active

        // Act
        sale.CancelItem(productId1);

        // Assert
        var cancelledItem = sale.Items.First(i => i.ProductId == productId1);
        cancelledItem.IsCancelled.Should().BeTrue();
        cancelledItem.TotalAmount.Should().Be(0);
        
        var activeItem = sale.Items.First(i => i.ProductId == productId2);
        activeItem.IsCancelled.Should().BeFalse();
        
        sale.TotalAmount.Should().Be(expectedTotalAmount);
    }

    /// <summary>
    /// Tests that cancelling an item in a cancelled sale throws an exception.
    /// </summary>
    [Fact(DisplayName = "Cancelling item in cancelled sale should throw exception")]
    public void Given_CancelledSale_When_ItemCancelled_Then_ShouldThrowException()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        var productId = Guid.NewGuid();
        sale.AddItem(productId, "Test Product", 1, 10.0m);
        sale.Cancel();

        // Act & Assert
        Assert.Throws<DomainException>(() => sale.CancelItem(productId))
            .Message.Should().Be("Cannot cancel items in a cancelled sale");
    }

    /// <summary>
    /// Tests that cancelling a non-existent item throws an exception.
    /// </summary>
    [Fact(DisplayName = "Cancelling non-existent item should throw exception")]
    public void Given_Sale_When_NonExistentItemCancelled_Then_ShouldThrowException()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        var nonExistentProductId = Guid.NewGuid();

        // Act & Assert
        Assert.Throws<DomainException>(() => sale.CancelItem(nonExistentProductId))
            .Message.Should().Be("Product not found in this sale");
    }

    /// <summary>
    /// Tests that updating customer information updates the sale's customer data.
    /// </summary>
    [Fact(DisplayName = "Updating customer info should update sale's customer data")]
    public void Given_Sale_When_CustomerInfoUpdated_Then_ShouldUpdateSaleCustomerData()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSaleWithItems(1);
        var newCustomerId = Guid.NewGuid();
        var newCustomerName = "New Customer Name";

        // Act
        sale.UpdateCustomerInfo(newCustomerId, newCustomerName);

        // Assert
        sale.CustomerId.Should().Be(newCustomerId);
        sale.CustomerName.Should().Be(newCustomerName);
    }

    /// <summary>
    /// Tests that updating customer information in a cancelled sale throws an exception.
    /// </summary>
    [Fact(DisplayName = "Updating customer info in cancelled sale should throw exception")]
    public void Given_CancelledSale_When_CustomerInfoUpdated_Then_ShouldThrowException()
    {
        // Arrange
        var sale = SaleTestData.GenerateCancelledSale();
        var newCustomerId = Guid.NewGuid();
        var newCustomerName = "New Customer Name";

        // Act & Assert
        Assert.Throws<DomainException>(() => sale.UpdateCustomerInfo(newCustomerId, newCustomerName))
            .Message.Should().Be("Cannot update a cancelled sale");
    }

    /// <summary>
    /// Tests that updating branch information updates the sale's branch data.
    /// </summary>
    [Fact(DisplayName = "Updating branch info should update sale's branch data")]
    public void Given_Sale_When_BranchInfoUpdated_Then_ShouldUpdateSaleBranchData()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSaleWithItems(1);
        var newBranchId = Guid.NewGuid();
        var newBranchName = "New Branch Name";

        // Act
        sale.UpdateBranchInfo(newBranchId, newBranchName);

        // Assert
        sale.BranchId.Should().Be(newBranchId);
        sale.BranchName.Should().Be(newBranchName);
    }

    /// <summary>
    /// Tests that updating branch information in a cancelled sale throws an exception.
    /// </summary>
    [Fact(DisplayName = "Updating branch info in cancelled sale should throw exception")]
    public void Given_CancelledSale_When_BranchInfoUpdated_Then_ShouldThrowException()
    {
        // Arrange
        var sale = SaleTestData.GenerateCancelledSale();
        var newBranchId = Guid.NewGuid();
        var newBranchName = "New Branch Name";

        // Act & Assert
        Assert.Throws<DomainException>(() => sale.UpdateBranchInfo(newBranchId, newBranchName))
            .Message.Should().Be("Cannot update a cancelled sale");
    }
}