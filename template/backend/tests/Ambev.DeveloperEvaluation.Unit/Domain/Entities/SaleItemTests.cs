using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Contains unit tests for the SaleItem entity class.
/// Tests cover business rules, status changes, and validation scenarios.
/// </summary>
public class SaleItemTests
{
    /// <summary>
    /// Tests that a new sale item is created with the correct initial values.
    /// </summary>
    [Fact(DisplayName = "New sale item should have correct initial values")]
    public void Given_NewSaleItem_When_Created_Then_ShouldHaveCorrectInitialValues()
    {
        // Arrange
        var saleId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var productName = "Test Product";
        var quantity = 2;
        var unitPrice = 10.0m;
        var discountPercentage = 0.0m;
        var expectedTotalAmount = quantity * unitPrice;

        // Act
        var saleItem = new SaleItem(saleId, productId, productName, quantity, unitPrice, discountPercentage);

        // Assert
        saleItem.SaleId.Should().Be(saleId);
        saleItem.ProductId.Should().Be(productId);
        saleItem.ProductName.Should().Be(productName);
        saleItem.Quantity.Should().Be(quantity);
        saleItem.UnitPrice.Should().Be(unitPrice);
        saleItem.DiscountPercentage.Should().Be(discountPercentage);
        saleItem.TotalAmount.Should().Be(expectedTotalAmount);
        saleItem.IsCancelled.Should().BeFalse();
    }

    /// <summary>
    /// Tests that a sale item with discount is created with the correct total amount.
    /// </summary>
    [Fact(DisplayName = "Sale item with discount should have correct total amount")]
    public void Given_SaleItemWithDiscount_When_Created_Then_ShouldHaveCorrectTotalAmount()
    {
        // Arrange
        var saleId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var productName = "Test Product";
        var quantity = 5;
        var unitPrice = 10.0m;
        var discountPercentage = 0.1m; // 10% discount
        var expectedTotalAmount = quantity * unitPrice * (1 - discountPercentage);

        // Act
        var saleItem = new SaleItem(saleId, productId, productName, quantity, unitPrice, discountPercentage);

        // Assert
        saleItem.TotalAmount.Should().Be(expectedTotalAmount);
    }

    /// <summary>
    /// Tests that updating a sale item's quantity updates the quantity and total amount.
    /// </summary>
    [Fact(DisplayName = "Updating quantity should update quantity and total amount")]
    public void Given_SaleItem_When_QuantityUpdated_Then_ShouldUpdateQuantityAndTotalAmount()
    {
        // Arrange
        var saleId = Guid.NewGuid();
        var saleItem = SaleTestData.GenerateValidSaleItem(saleId, 2);
        var newQuantity = 5;
        var newDiscountPercentage = 0.1m; // 10% discount for quantity 4-9
        var expectedTotalAmount = newQuantity * saleItem.UnitPrice * (1 - newDiscountPercentage);

        // Act
        saleItem.UpdateQuantity(newQuantity, newDiscountPercentage);

        // Assert
        saleItem.Quantity.Should().Be(newQuantity);
        saleItem.DiscountPercentage.Should().Be(newDiscountPercentage);
        saleItem.TotalAmount.Should().Be(expectedTotalAmount);
    }

    /// <summary>
    /// Tests that updating a cancelled sale item throws an exception.
    /// </summary>
    [Fact(DisplayName = "Updating cancelled sale item should throw exception")]
    public void Given_CancelledSaleItem_When_QuantityUpdated_Then_ShouldThrowException()
    {
        // Arrange
        var saleId = Guid.NewGuid();
        var saleItem = SaleTestData.GenerateValidSaleItem(saleId);
        saleItem.Cancel();

        // Act & Assert
        Assert.Throws<DomainException>(() => saleItem.UpdateQuantity(5, 0.1m))
            .Message.Should().Be("Cannot update a cancelled item");
    }

    /// <summary>
    /// Tests that cancelling a sale item sets IsCancelled to true and TotalAmount to 0.
    /// </summary>
    [Fact(DisplayName = "Cancelling sale item should set IsCancelled to true and TotalAmount to 0")]
    public void Given_SaleItem_When_Cancelled_Then_ShouldSetIsCancelledToTrueAndTotalAmountToZero()
    {
        // Arrange
        var saleId = Guid.NewGuid();
        var saleItem = SaleTestData.GenerateValidSaleItem(saleId, 5);
        var initialTotalAmount = saleItem.TotalAmount;
        initialTotalAmount.Should().BeGreaterThan(0); // Verify initial total amount is greater than 0

        // Act
        saleItem.Cancel();

        // Assert
        saleItem.IsCancelled.Should().BeTrue();
        saleItem.TotalAmount.Should().Be(0);
    }
}