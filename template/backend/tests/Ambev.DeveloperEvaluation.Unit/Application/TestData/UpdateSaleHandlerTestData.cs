using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain;

/// <summary>
/// Provides methods for generating test data for UpdateSaleHandler tests using the Bogus library.
/// This class centralizes all test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class UpdateSaleHandlerTestData
{
    /// <summary>
    /// Configures the Faker to generate valid UpdateSaleItemCommand entities.
    /// </summary>
    private static readonly Faker<UpdateSaleItemCommand> updateSaleItemCommandFaker = new Faker<UpdateSaleItemCommand>()
        .RuleFor(i => i.ProductId, f => Guid.NewGuid())
        .RuleFor(i => i.ProductName, f => f.Commerce.ProductName())
        .RuleFor(i => i.Quantity, f => f.Random.Int(1, 20))
        .RuleFor(i => i.UnitPrice, f => decimal.Parse(f.Commerce.Price(min: 10, max: 1000)));

    /// <summary>
    /// Configures the Faker to generate valid UpdateSaleCommand entities.
    /// </summary>
    private static readonly Faker<UpdateSaleCommand> updateSaleCommandFaker = new Faker<UpdateSaleCommand>()
        .RuleFor(s => s.Id, f => Guid.NewGuid())
        .RuleFor(s => s.CustomerId, f => Guid.NewGuid())
        .RuleFor(s => s.CustomerName, f => f.Name.FullName())
        .RuleFor(s => s.BranchId, f => Guid.NewGuid())
        .RuleFor(s => s.BranchName, f => f.Company.CompanyName())
        .RuleFor(s => s.Items, f => updateSaleItemCommandFaker.Generate(f.Random.Int(1, 5)).ToList());

    /// <summary>
    /// Generates a valid UpdateSaleCommand with randomized data.
    /// </summary>
    /// <returns>A valid UpdateSaleCommand with randomly generated data.</returns>
    public static UpdateSaleCommand GenerateValidCommand()
    {
        return updateSaleCommandFaker.Generate();
    }

    /// <summary>
    /// Generates a valid UpdateSaleCommand with a specific ID.
    /// </summary>
    /// <param name="id">The ID to set for the command.</param>
    /// <returns>A valid UpdateSaleCommand with the specified ID.</returns>
    public static UpdateSaleCommand GenerateValidCommandWithId(Guid id)
    {
        var command = updateSaleCommandFaker.Generate();
        command.Id = id;
        return command;
    }

    /// <summary>
    /// Generates a valid UpdateSaleCommand with a specific number of items.
    /// </summary>
    /// <param name="itemCount">The number of items to include in the sale.</param>
    /// <returns>A valid UpdateSaleCommand with the specified number of items.</returns>
    public static UpdateSaleCommand GenerateValidCommandWithItemCount(int itemCount)
    {
        var command = updateSaleCommandFaker.Generate();
        command.Items = updateSaleItemCommandFaker.Generate(itemCount).ToList();
        return command;
    }

    /// <summary>
    /// Generates a UpdateSaleCommand with items that have specific quantities.
    /// </summary>
    /// <param name="quantities">The quantities to set for each item.</param>
    /// <returns>A UpdateSaleCommand with items having the specified quantities.</returns>
    public static UpdateSaleCommand GenerateCommandWithItemQuantities(params int[] quantities)
    {
        var command = updateSaleCommandFaker.Generate();
        command.Items.Clear();

        foreach (var quantity in quantities)
        {
            var item = updateSaleItemCommandFaker.Generate();
            item.Quantity = quantity;
            command.Items.Add(item);
        }

        return command;
    }

    /// <summary>
    /// Generates an invalid UpdateSaleCommand with no items.
    /// </summary>
    /// <returns>An invalid UpdateSaleCommand with no items.</returns>
    public static UpdateSaleCommand GenerateInvalidCommandWithNoItems()
    {
        var command = updateSaleCommandFaker.Generate();
        command.Items.Clear();
        return command;
    }

    /// <summary>
    /// Generates an invalid UpdateSaleCommand with empty customer name.
    /// </summary>
    /// <returns>An invalid UpdateSaleCommand with empty customer name.</returns>
    public static UpdateSaleCommand GenerateInvalidCommandWithEmptyCustomerName()
    {
        var command = updateSaleCommandFaker.Generate();
        command.CustomerName = string.Empty;
        return command;
    }

    /// <summary>
    /// Generates an invalid UpdateSaleCommand with items exceeding maximum quantity.
    /// </summary>
    /// <returns>An invalid UpdateSaleCommand with items exceeding maximum quantity.</returns>
    public static UpdateSaleCommand GenerateInvalidCommandWithExcessiveQuantity()
    {
        var command = updateSaleCommandFaker.Generate();
        command.Items[0].Quantity = 21; // Exceeds maximum of 20
        return command;
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
}