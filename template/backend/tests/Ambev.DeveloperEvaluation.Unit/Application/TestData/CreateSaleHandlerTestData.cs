using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain;

/// <summary>
/// Provides methods for generating test data for CreateSaleHandler tests using the Bogus library.
/// This class centralizes all test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class CreateSaleHandlerTestData
{
    /// <summary>
    /// Configures the Faker to generate valid CreateSaleItemCommand entities.
    /// </summary>
    private static readonly Faker<CreateSaleItemCommand> createSaleItemCommandFaker = new Faker<CreateSaleItemCommand>()
        .RuleFor(i => i.ProductId, f => Guid.NewGuid())
        .RuleFor(i => i.ProductName, f => f.Commerce.ProductName())
        .RuleFor(i => i.Quantity, f => f.Random.Int(1, 20))
        .RuleFor(i => i.UnitPrice, f => decimal.Parse(f.Commerce.Price(min: 10, max: 1000)));

    /// <summary>
    /// Configures the Faker to generate valid CreateSaleCommand entities.
    /// </summary>
    private static readonly Faker<CreateSaleCommand> createSaleCommandFaker = new Faker<CreateSaleCommand>()
        .RuleFor(s => s.CustomerId, f => Guid.NewGuid())
        .RuleFor(s => s.CustomerName, f => f.Name.FullName())
        .RuleFor(s => s.BranchId, f => Guid.NewGuid())
        .RuleFor(s => s.BranchName, f => f.Company.CompanyName())
        .RuleFor(s => s.Items, f => createSaleItemCommandFaker.Generate(f.Random.Int(1, 5)).ToList());

    /// <summary>
    /// Generates a valid CreateSaleCommand with randomized data.
    /// </summary>
    /// <returns>A valid CreateSaleCommand with randomly generated data.</returns>
    public static CreateSaleCommand GenerateValidCommand()
    {
        return createSaleCommandFaker.Generate();
    }

    /// <summary>
    /// Generates a valid CreateSaleCommand with a specific number of items.
    /// </summary>
    /// <param name="itemCount">The number of items to include in the sale.</param>
    /// <returns>A valid CreateSaleCommand with the specified number of items.</returns>
    public static CreateSaleCommand GenerateValidCommandWithItemCount(int itemCount)
    {
        var command = createSaleCommandFaker.Generate();
        command.Items = createSaleItemCommandFaker.Generate(itemCount).ToList();
        return command;
    }

    /// <summary>
    /// Generates a CreateSaleCommand with items that have specific quantities.
    /// </summary>
    /// <param name="quantities">The quantities to set for each item.</param>
    /// <returns>A CreateSaleCommand with items having the specified quantities.</returns>
    public static CreateSaleCommand GenerateCommandWithItemQuantities(params int[] quantities)
    {
        var command = createSaleCommandFaker.Generate();
        command.Items.Clear();

        foreach (var quantity in quantities)
        {
            var item = createSaleItemCommandFaker.Generate();
            item.Quantity = quantity;
            command.Items.Add(item);
        }

        return command;
    }

    /// <summary>
    /// Generates an invalid CreateSaleCommand with no items.
    /// </summary>
    /// <returns>An invalid CreateSaleCommand with no items.</returns>
    public static CreateSaleCommand GenerateInvalidCommandWithNoItems()
    {
        var command = createSaleCommandFaker.Generate();
        command.Items.Clear();
        return command;
    }

    /// <summary>
    /// Generates an invalid CreateSaleCommand with empty customer name.
    /// </summary>
    /// <returns>An invalid CreateSaleCommand with empty customer name.</returns>
    public static CreateSaleCommand GenerateInvalidCommandWithEmptyCustomerName()
    {
        var command = createSaleCommandFaker.Generate();
        command.CustomerName = string.Empty;
        return command;
    }

    /// <summary>
    /// Generates an invalid CreateSaleCommand with items exceeding maximum quantity.
    /// </summary>
    /// <returns>An invalid CreateSaleCommand with items exceeding maximum quantity.</returns>
    public static CreateSaleCommand GenerateInvalidCommandWithExcessiveQuantity()
    {
        var command = createSaleCommandFaker.Generate();
        command.Items[0].Quantity = 21; // Exceeds maximum of 20
        return command;
    }
}