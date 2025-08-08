using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain;

/// <summary>
/// Provides methods for generating test data for GetSaleHandler tests using the Bogus library.
/// This class centralizes all test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class GetSaleHandlerTestData
{
    /// <summary>
    /// Generates a valid GetSaleCommand with randomized data.
    /// </summary>
    /// <returns>A valid GetSaleCommand with randomly generated data.</returns>
    public static GetSaleCommand GenerateValidCommand()
    {
        return new GetSaleCommand(Guid.NewGuid());
    }

    /// <summary>
    /// Generates a valid GetSaleCommand with a specific ID.
    /// </summary>
    /// <param name="id">The ID to set for the command.</param>
    /// <returns>A valid GetSaleCommand with the specified ID.</returns>
    public static GetSaleCommand GenerateValidCommandWithId(Guid id)
    {
        return new GetSaleCommand(id);
    }

    /// <summary>
    /// Generates an invalid GetSaleCommand with an empty GUID.
    /// </summary>
    /// <returns>An invalid GetSaleCommand with an empty GUID.</returns>
    public static GetSaleCommand GenerateInvalidCommand()
    {
        return new GetSaleCommand(Guid.Empty);
    }

    /// <summary>
    /// Generates a sample Sale entity for testing.
    /// </summary>
    /// <returns>A sample Sale entity.</returns>
    public static Sale GenerateSampleSale()
    {
        var faker = new Faker();
        var saleId = Guid.NewGuid();
        var customerId = Guid.NewGuid();
        var branchId = Guid.NewGuid();
        
        var sale = new Sale(
            $"SALE-{DateTime.Now:yyyyMMdd}-{faker.Random.Number(100000, 999999)}",
            customerId,
            faker.Name.FullName(),
            branchId,
            faker.Company.CompanyName()
        );

        // Add some items to the sale
        for (int i = 0; i < faker.Random.Int(1, 3); i++)
        {
            sale.AddItem(
                Guid.NewGuid(),
                faker.Commerce.ProductName(),
                faker.Random.Int(1, 20),
                decimal.Parse(faker.Commerce.Price(min: 10, max: 1000))
            );
        }

        // Use reflection to set the ID since it's a private setter
        typeof(Sale).GetProperty("Id").SetValue(sale, saleId);

        return sale;
    }

    /// <summary>
    /// Generates a sample cancelled Sale entity for testing.
    /// </summary>
    /// <returns>A sample cancelled Sale entity.</returns>
    public static Sale GenerateCancelledSale()
    {
        var sale = GenerateSampleSale();
        sale.Cancel();
        return sale;
    }

    /// <summary>
    /// Generates a sample GetSaleResult based on a Sale entity.
    /// </summary>
    /// <param name="sale">The Sale entity to map from.</param>
    /// <returns>A GetSaleResult mapped from the Sale entity.</returns>
    public static GetSaleResult GenerateGetSaleResult(Sale sale)
    {
        var result = new GetSaleResult
        {
            Id = sale.Id,
            SaleNumber = sale.SaleNumber,
            SaleDate = sale.SaleDate,
            CustomerId = sale.CustomerId,
            CustomerName = sale.CustomerName,
            BranchId = sale.BranchId,
            BranchName = sale.BranchName,
            TotalAmount = sale.TotalAmount,
            Status = sale.Status,
            CreatedAt = sale.CreatedAt,
            UpdatedAt = sale.UpdatedAt
        };

        foreach (var item in sale.Items)
        {
            result.Items.Add(new SaleItemResult
            {
                Id = item.Id,
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                DiscountPercentage = item.DiscountPercentage,
                TotalAmount = item.TotalAmount,
                IsCancelled = item.IsCancelled
            });
        }

        return result;
    }
}