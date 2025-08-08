using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Validation;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentValidation.TestHelper;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Validation;

/// <summary>
/// Contains unit tests for the SaleItemValidator class.
/// Tests cover validation of all sale item properties including product info,
/// quantity, unit price, discount percentage, and total amount requirements.
/// </summary>
public class SaleItemValidatorTests
{
    private readonly SaleItemValidator _validator;

    public SaleItemValidatorTests()
    {
        _validator = new SaleItemValidator();
    }

    /// <summary>
    /// Tests that validation passes when all sale item properties are valid.
    /// </summary>
    [Fact(DisplayName = "Valid sale item should pass all validation rules")]
    public void Given_ValidSaleItem_When_Validated_Then_ShouldNotHaveErrors()
    {
        // Arrange
        var saleId = System.Guid.NewGuid();
        var saleItem = SaleTestData.GenerateValidSaleItem(saleId, 5);

        // Act
        var result = _validator.TestValidate(saleItem);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    /// <summary>
    /// Tests that validation fails for empty sale ID.
    /// </summary>
    [Fact(DisplayName = "Empty sale ID should fail validation")]
    public void Given_EmptySaleId_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var saleId = System.Guid.NewGuid();
        var saleItem = SaleTestData.GenerateValidSaleItem(saleId);
        // Use reflection to set the private property
        typeof(SaleItem).GetProperty("SaleId").SetValue(saleItem, System.Guid.Empty);

        // Act
        var result = _validator.TestValidate(saleItem);

        // Assert
        result.ShouldHaveValidationErrorFor(si => si.SaleId);
    }

    /// <summary>
    /// Tests that validation fails for empty product ID.
    /// </summary>
    [Fact(DisplayName = "Empty product ID should fail validation")]
    public void Given_EmptyProductId_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var saleId = System.Guid.NewGuid();
        var saleItem = SaleTestData.GenerateValidSaleItem(saleId);
        // Use reflection to set the private property
        typeof(SaleItem).GetProperty("ProductId").SetValue(saleItem, System.Guid.Empty);

        // Act
        var result = _validator.TestValidate(saleItem);

        // Assert
        result.ShouldHaveValidationErrorFor(si => si.ProductId);
    }

    /// <summary>
    /// Tests that validation fails for empty product name.
    /// </summary>
    [Fact(DisplayName = "Empty product name should fail validation")]
    public void Given_EmptyProductName_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var saleId = System.Guid.NewGuid();
        var saleItem = SaleTestData.GenerateValidSaleItem(saleId);
        // Use reflection to set the private property
        typeof(SaleItem).GetProperty("ProductName").SetValue(saleItem, string.Empty);

        // Act
        var result = _validator.TestValidate(saleItem);

        // Assert
        result.ShouldHaveValidationErrorFor(si => si.ProductName);
    }

    /// <summary>
    /// Tests that validation fails for product name exceeding maximum length.
    /// </summary>
    [Fact(DisplayName = "Product name exceeding maximum length should fail validation")]
    public void Given_ProductNameExceedingMaxLength_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var saleId = System.Guid.NewGuid();
        var saleItem = SaleTestData.GenerateValidSaleItem(saleId);
        // Use reflection to set the private property
        typeof(SaleItem).GetProperty("ProductName").SetValue(saleItem, new string('A', 101)); // 101 characters

        // Act
        var result = _validator.TestValidate(saleItem);

        // Assert
        result.ShouldHaveValidationErrorFor(si => si.ProductName);
    }

    /// <summary>
    /// Tests that validation fails for zero quantity.
    /// </summary>
    [Fact(DisplayName = "Zero quantity should fail validation")]
    public void Given_ZeroQuantity_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var saleId = System.Guid.NewGuid();
        var saleItem = SaleTestData.GenerateValidSaleItem(saleId);
        // Use reflection to set the private property
        typeof(SaleItem).GetProperty("Quantity").SetValue(saleItem, 0);

        // Act
        var result = _validator.TestValidate(saleItem);

        // Assert
        result.ShouldHaveValidationErrorFor(si => si.Quantity);
    }

    /// <summary>
    /// Tests that validation fails for negative quantity.
    /// </summary>
    [Fact(DisplayName = "Negative quantity should fail validation")]
    public void Given_NegativeQuantity_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var saleId = System.Guid.NewGuid();
        var saleItem = SaleTestData.GenerateValidSaleItem(saleId);
        // Use reflection to set the private property
        typeof(SaleItem).GetProperty("Quantity").SetValue(saleItem, -1);

        // Act
        var result = _validator.TestValidate(saleItem);

        // Assert
        result.ShouldHaveValidationErrorFor(si => si.Quantity);
    }

    /// <summary>
    /// Tests that validation fails for quantity exceeding maximum limit.
    /// </summary>
    [Fact(DisplayName = "Quantity exceeding maximum limit should fail validation")]
    public void Given_QuantityExceedingMaxLimit_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var saleId = System.Guid.NewGuid();
        var saleItem = SaleTestData.GenerateValidSaleItem(saleId);
        // Use reflection to set the private property
        typeof(SaleItem).GetProperty("Quantity").SetValue(saleItem, 21); // Max is 20

        // Act
        var result = _validator.TestValidate(saleItem);

        // Assert
        result.ShouldHaveValidationErrorFor(si => si.Quantity);
    }

    /// <summary>
    /// Tests that validation fails for zero unit price.
    /// </summary>
    [Fact(DisplayName = "Zero unit price should fail validation")]
    public void Given_ZeroUnitPrice_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var saleId = System.Guid.NewGuid();
        var saleItem = SaleTestData.GenerateValidSaleItem(saleId);
        // Use reflection to set the private property
        typeof(SaleItem).GetProperty("UnitPrice").SetValue(saleItem, 0m);

        // Act
        var result = _validator.TestValidate(saleItem);

        // Assert
        result.ShouldHaveValidationErrorFor(si => si.UnitPrice);
    }

    /// <summary>
    /// Tests that validation fails for negative unit price.
    /// </summary>
    [Fact(DisplayName = "Negative unit price should fail validation")]
    public void Given_NegativeUnitPrice_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var saleId = System.Guid.NewGuid();
        var saleItem = SaleTestData.GenerateValidSaleItem(saleId);
        // Use reflection to set the private property
        typeof(SaleItem).GetProperty("UnitPrice").SetValue(saleItem, -1m);

        // Act
        var result = _validator.TestValidate(saleItem);

        // Assert
        result.ShouldHaveValidationErrorFor(si => si.UnitPrice);
    }

    /// <summary>
    /// Tests that validation fails for negative discount percentage.
    /// </summary>
    [Fact(DisplayName = "Negative discount percentage should fail validation")]
    public void Given_NegativeDiscountPercentage_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var saleId = System.Guid.NewGuid();
        var saleItem = SaleTestData.GenerateValidSaleItem(saleId);
        // Use reflection to set the private property
        typeof(SaleItem).GetProperty("DiscountPercentage").SetValue(saleItem, -0.1m);

        // Act
        var result = _validator.TestValidate(saleItem);

        // Assert
        result.ShouldHaveValidationErrorFor(si => si.DiscountPercentage);
    }

    /// <summary>
    /// Tests that validation fails for discount percentage exceeding 100%.
    /// </summary>
    [Fact(DisplayName = "Discount percentage exceeding 100% should fail validation")]
    public void Given_DiscountPercentageExceeding100Percent_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var saleId = System.Guid.NewGuid();
        var saleItem = SaleTestData.GenerateValidSaleItem(saleId);
        // Use reflection to set the private property
        typeof(SaleItem).GetProperty("DiscountPercentage").SetValue(saleItem, 1.1m); // 110%

        // Act
        var result = _validator.TestValidate(saleItem);

        // Assert
        result.ShouldHaveValidationErrorFor(si => si.DiscountPercentage);
    }

    /// <summary>
    /// Tests that validation fails for negative total amount.
    /// </summary>
    [Fact(DisplayName = "Negative total amount should fail validation")]
    public void Given_NegativeTotalAmount_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var saleId = System.Guid.NewGuid();
        var saleItem = SaleTestData.GenerateValidSaleItem(saleId);
        // Use reflection to set the private property
        typeof(SaleItem).GetProperty("TotalAmount").SetValue(saleItem, -1m);

        // Act
        var result = _validator.TestValidate(saleItem);

        // Assert
        result.ShouldHaveValidationErrorFor(si => si.TotalAmount);
    }

    /// <summary>
    /// Tests that validation fails when discount is applied to items with quantity less than 4.
    /// </summary>
    [Fact(DisplayName = "Discount on items with quantity less than 4 should fail validation")]
    public void Given_DiscountOnItemsWithQuantityLessThan4_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var saleId = System.Guid.NewGuid();
        var saleItem = SaleTestData.GenerateValidSaleItem(saleId, 3); // Quantity less than 4
        // Use reflection to set the private property
        typeof(SaleItem).GetProperty("DiscountPercentage").SetValue(saleItem, 0.1m); // 10% discount

        // Act
        var result = _validator.TestValidate(saleItem);

        // Assert
        result.ShouldHaveValidationErrorFor(si => si.DiscountPercentage);
    }

    /// <summary>
    /// Tests that validation fails when incorrect discount is applied to items with quantity between 4 and 9.
    /// </summary>
    [Fact(DisplayName = "Incorrect discount on items with quantity 4-9 should fail validation")]
    public void Given_IncorrectDiscountOnItemsWithQuantity4To9_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var saleId = System.Guid.NewGuid();
        var saleItem = SaleTestData.GenerateValidSaleItem(saleId, 5); // Quantity between 4 and 9
        // Use reflection to set the private property
        typeof(SaleItem).GetProperty("DiscountPercentage").SetValue(saleItem, 0.2m); // 20% discount instead of 10%

        // Act
        var result = _validator.TestValidate(saleItem);

        // Assert
        result.ShouldHaveValidationErrorFor(si => si.DiscountPercentage);
    }

    /// <summary>
    /// Tests that validation fails when incorrect discount is applied to items with quantity between 10 and 20.
    /// </summary>
    [Fact(DisplayName = "Incorrect discount on items with quantity 10-20 should fail validation")]
    public void Given_IncorrectDiscountOnItemsWithQuantity10To20_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var saleId = System.Guid.NewGuid();
        var saleItem = SaleTestData.GenerateValidSaleItem(saleId, 15); // Quantity between 10 and 20
        // Use reflection to set the private property
        typeof(SaleItem).GetProperty("DiscountPercentage").SetValue(saleItem, 0.1m); // 10% discount instead of 20%

        // Act
        var result = _validator.TestValidate(saleItem);

        // Assert
        result.ShouldHaveValidationErrorFor(si => si.DiscountPercentage);
    }
}