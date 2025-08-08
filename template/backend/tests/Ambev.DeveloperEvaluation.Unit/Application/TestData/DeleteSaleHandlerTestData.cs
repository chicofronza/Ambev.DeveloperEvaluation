using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain;

/// <summary>
/// Provides methods for generating test data for DeleteSaleHandler tests using the Bogus library.
/// This class centralizes all test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class DeleteSaleHandlerTestData
{
    /// <summary>
    /// Generates a valid DeleteSaleCommand with randomized data.
    /// </summary>
    /// <returns>A valid DeleteSaleCommand with randomly generated data.</returns>
    public static DeleteSaleCommand GenerateValidCommand()
    {
        return new DeleteSaleCommand(Guid.NewGuid());
    }

    /// <summary>
    /// Generates a valid DeleteSaleCommand with a specific ID.
    /// </summary>
    /// <param name="id">The ID to set for the command.</param>
    /// <returns>A valid DeleteSaleCommand with the specified ID.</returns>
    public static DeleteSaleCommand GenerateValidCommandWithId(Guid id)
    {
        return new DeleteSaleCommand(id);
    }

    /// <summary>
    /// Generates a valid DeleteSaleCommand with physical deletion enabled.
    /// </summary>
    /// <param name="id">The ID to set for the command.</param>
    /// <returns>A valid DeleteSaleCommand with physical deletion enabled.</returns>
    public static DeleteSaleCommand GenerateValidCommandWithPhysicalDeletion(Guid id)
    {
        return new DeleteSaleCommand(id, true);
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
}