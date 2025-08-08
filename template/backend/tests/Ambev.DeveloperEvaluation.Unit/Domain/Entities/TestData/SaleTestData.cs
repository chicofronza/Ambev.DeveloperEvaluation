using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

/// <summary>
/// Provides methods for generating test data for Sale entity tests using the Bogus library.
/// This class centralizes all test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class SaleTestData
{
    /// <summary>
    /// Generates a valid Sale entity with randomized data.
    /// </summary>
    /// <returns>A valid Sale entity with randomly generated data.</returns>
    public static Sale GenerateValidSale()
    {
        var faker = new Faker();
        var saleNumber = $"SALE-{DateTime.Now:yyyyMMdd}-{faker.Random.Number(100000, 999999)}";
        var customerId = Guid.NewGuid();
        var customerName = faker.Name.FullName();
        var branchId = Guid.NewGuid();
        var branchName = faker.Company.CompanyName();

        return new Sale(saleNumber, customerId, customerName, branchId, branchName);
    }

    /// <summary>
    /// Generates a valid Sale entity with the specified number of items.
    /// </summary>
    /// <param name="itemCount">The number of items to add to the sale.</param>
    /// <returns>A valid Sale entity with the specified number of items.</returns>
    public static Sale GenerateValidSaleWithItems(int itemCount)
    {
        var sale = GenerateValidSale();
        var faker = new Faker();

        for (int i = 0; i < itemCount; i++)
        {
            sale.AddItem(
                Guid.NewGuid(),
                faker.Commerce.ProductName(),
                faker.Random.Int(1, 20),
                decimal.Parse(faker.Commerce.Price(min: 10, max: 1000))
            );
        }

        return sale;
    }

    /// <summary>
    /// Generates a valid Sale entity with items having specific quantities.
    /// </summary>
    /// <param name="quantities">The quantities to set for each item.</param>
    /// <returns>A valid Sale entity with items having the specified quantities.</returns>
    public static Sale GenerateSaleWithItemQuantities(params int[] quantities)
    {
        var sale = GenerateValidSale();
        var faker = new Faker();

        foreach (var quantity in quantities)
        {
            sale.AddItem(
                Guid.NewGuid(),
                faker.Commerce.ProductName(),
                quantity,
                decimal.Parse(faker.Commerce.Price(min: 10, max: 1000))
            );
        }

        return sale;
    }

    /// <summary>
    /// Generates a cancelled Sale entity.
    /// </summary>
    /// <returns>A cancelled Sale entity.</returns>
    public static Sale GenerateCancelledSale()
    {
        var sale = GenerateValidSaleWithItems(3);
        sale.Cancel();
        return sale;
    }

    /// <summary>
    /// Generates a valid SaleItem for testing.
    /// </summary>
    /// <param name="saleId">The ID of the sale the item belongs to.</param>
    /// <param name="quantity">The quantity of the item.</param>
    /// <returns>A valid SaleItem with the specified parameters.</returns>
    public static SaleItem GenerateValidSaleItem(Guid saleId, int quantity = 1)
    {
        var faker = new Faker();
        var productId = Guid.NewGuid();
        var productName = faker.Commerce.ProductName();
        var unitPrice = decimal.Parse(faker.Commerce.Price(min: 10, max: 1000));
        
        // Calculate discount based on quantity
        decimal discountPercentage = 0;
        if (quantity >= 10 && quantity <= 20)
        {
            discountPercentage = 0.2m; // 20% discount for 10-20 items
        }
        else if (quantity >= 4)
        {
            discountPercentage = 0.1m; // 10% discount for 4-9 items
        }

        return new SaleItem(saleId, productId, productName, quantity, unitPrice, discountPercentage);
    }

    /// <summary>
    /// Generates a valid sale number.
    /// </summary>
    /// <returns>A valid sale number.</returns>
    public static string GenerateValidSaleNumber()
    {
        var faker = new Faker();
        return $"SALE-{DateTime.Now:yyyyMMdd}-{faker.Random.Number(100000, 999999)}";
    }

    /// <summary>
    /// Generates an invalid sale number.
    /// </summary>
    /// <returns>An invalid sale number.</returns>
    public static string GenerateInvalidSaleNumber()
    {
        return string.Empty;
    }

    /// <summary>
    /// Generates a valid customer name.
    /// </summary>
    /// <returns>A valid customer name.</returns>
    public static string GenerateValidCustomerName()
    {
        return new Faker().Name.FullName();
    }

    /// <summary>
    /// Generates an invalid customer name.
    /// </summary>
    /// <returns>An invalid customer name.</returns>
    public static string GenerateInvalidCustomerName()
    {
        return string.Empty;
    }

    /// <summary>
    /// Generates a valid branch name.
    /// </summary>
    /// <returns>A valid branch name.</returns>
    public static string GenerateValidBranchName()
    {
        return new Faker().Company.CompanyName();
    }

    /// <summary>
    /// Generates an invalid branch name.
    /// </summary>
    /// <returns>An invalid branch name.</returns>
    public static string GenerateInvalidBranchName()
    {
        return string.Empty;
    }
}