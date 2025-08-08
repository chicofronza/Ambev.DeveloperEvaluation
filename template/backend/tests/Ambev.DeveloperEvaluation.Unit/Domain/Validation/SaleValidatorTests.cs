using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Validation;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentValidation.TestHelper;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Validation;

/// <summary>
/// Contains unit tests for the SaleValidator class.
/// Tests cover validation of all sale properties including sale number, customer info,
/// branch info, total amount, status, and items requirements.
/// </summary>
public class SaleValidatorTests
{
    private readonly SaleValidator _validator;

    public SaleValidatorTests()
    {
        _validator = new SaleValidator();
    }

    /// <summary>
    /// Tests that validation fails for empty sale number.
    /// </summary>
    [Fact(DisplayName = "Empty sale number should fail validation")]
    public void Given_EmptySaleNumber_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSaleWithItems(1);
        // Use reflection to set the private property
        typeof(Sale).GetProperty("SaleNumber").SetValue(sale, string.Empty);

        // Act
        var result = _validator.TestValidate(sale);

        // Assert
        result.ShouldHaveValidationErrorFor(s => s.SaleNumber);
    }

    /// <summary>
    /// Tests that validation fails for empty customer ID.
    /// </summary>
    [Fact(DisplayName = "Empty customer ID should fail validation")]
    public void Given_EmptyCustomerId_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSaleWithItems(1);
        // Use reflection to set the private property
        typeof(Sale).GetProperty("CustomerId").SetValue(sale, Guid.Empty);

        // Act
        var result = _validator.TestValidate(sale);

        // Assert
        result.ShouldHaveValidationErrorFor(s => s.CustomerId);
    }

    /// <summary>
    /// Tests that validation fails for empty customer name.
    /// </summary>
    [Fact(DisplayName = "Empty customer name should fail validation")]
    public void Given_EmptyCustomerName_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSaleWithItems(1);
        // Use reflection to set the private property
        typeof(Sale).GetProperty("CustomerName").SetValue(sale, string.Empty);

        // Act
        var result = _validator.TestValidate(sale);

        // Assert
        result.ShouldHaveValidationErrorFor(s => s.CustomerName);
    }

    /// <summary>
    /// Tests that validation fails for customer name exceeding maximum length.
    /// </summary>
    [Fact(DisplayName = "Customer name exceeding maximum length should fail validation")]
    public void Given_CustomerNameExceedingMaxLength_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSaleWithItems(1);
        // Use reflection to set the private property
        typeof(Sale).GetProperty("CustomerName").SetValue(sale, new string('A', 101)); // 101 characters

        // Act
        var result = _validator.TestValidate(sale);

        // Assert
        result.ShouldHaveValidationErrorFor(s => s.CustomerName);
    }

    /// <summary>
    /// Tests that validation fails for empty branch ID.
    /// </summary>
    [Fact(DisplayName = "Empty branch ID should fail validation")]
    public void Given_EmptyBranchId_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSaleWithItems(1);
        // Use reflection to set the private property
        typeof(Sale).GetProperty("BranchId").SetValue(sale, Guid.Empty);

        // Act
        var result = _validator.TestValidate(sale);

        // Assert
        result.ShouldHaveValidationErrorFor(s => s.BranchId);
    }

    /// <summary>
    /// Tests that validation fails for empty branch name.
    /// </summary>
    [Fact(DisplayName = "Empty branch name should fail validation")]
    public void Given_EmptyBranchName_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSaleWithItems(1);
        // Use reflection to set the private property
        typeof(Sale).GetProperty("BranchName").SetValue(sale, string.Empty);

        // Act
        var result = _validator.TestValidate(sale);

        // Assert
        result.ShouldHaveValidationErrorFor(s => s.BranchName);
    }

    /// <summary>
    /// Tests that validation fails for branch name exceeding maximum length.
    /// </summary>
    [Fact(DisplayName = "Branch name exceeding maximum length should fail validation")]
    public void Given_BranchNameExceedingMaxLength_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSaleWithItems(1);
        // Use reflection to set the private property
        typeof(Sale).GetProperty("BranchName").SetValue(sale, new string('A', 101)); // 101 characters

        // Act
        var result = _validator.TestValidate(sale);

        // Assert
        result.ShouldHaveValidationErrorFor(s => s.BranchName);
    }

    /// <summary>
    /// Tests that validation fails for negative total amount.
    /// </summary>
    [Fact(DisplayName = "Negative total amount should fail validation")]
    public void Given_NegativeTotalAmount_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSaleWithItems(1);
        // Use reflection to set the private property
        typeof(Sale).GetProperty("TotalAmount").SetValue(sale, -1m);

        // Act
        var result = _validator.TestValidate(sale);

        // Assert
        result.ShouldHaveValidationErrorFor(s => s.TotalAmount);
    }

    /// <summary>
    /// Tests that validation fails for invalid sale status.
    /// </summary>
    [Fact(DisplayName = "Invalid sale status should fail validation")]
    public void Given_InvalidSaleStatus_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSaleWithItems(1);
        // Use reflection to set the private property with an invalid enum value
        typeof(Sale).GetProperty("Status").SetValue(sale, (SaleStatus)999);

        // Act
        var result = _validator.TestValidate(sale);

        // Assert
        result.ShouldHaveValidationErrorFor(s => s.Status);
    }

    /// <summary>
    /// Tests that validation fails for sale with no items.
    /// </summary>
    [Fact(DisplayName = "Sale with no items should fail validation")]
    public void Given_SaleWithNoItems_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var sale = SaleTestData.GenerateValidSale();
        // Sale is created without items

        // Act
        var result = _validator.TestValidate(sale);

        // Assert
        result.ShouldHaveValidationErrorFor(s => s.Items);
    }
}