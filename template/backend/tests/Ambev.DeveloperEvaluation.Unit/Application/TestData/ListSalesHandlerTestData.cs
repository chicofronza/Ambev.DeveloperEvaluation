using Ambev.DeveloperEvaluation.Application.Sales.ListSales;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Bogus;
using System.Collections.Generic;

namespace Ambev.DeveloperEvaluation.Unit.Domain;

/// <summary>
/// Provides methods for generating test data for ListSalesHandler tests using the Bogus library.
/// This class centralizes all test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class ListSalesHandlerTestData
{
    /// <summary>
    /// Generates a valid ListSalesCommand with no filters.
    /// </summary>
    /// <returns>A valid ListSalesCommand with no filters.</returns>
    public static ListSalesCommand GenerateValidCommand()
    {
        return new ListSalesCommand();
    }

    /// <summary>
    /// Generates a valid ListSalesCommand with customer ID filter.
    /// </summary>
    /// <param name="customerId">The customer ID to filter by.</param>
    /// <returns>A valid ListSalesCommand with customer ID filter.</returns>
    public static ListSalesCommand GenerateCommandWithCustomerFilter(Guid customerId)
    {
        return new ListSalesCommand { CustomerId = customerId };
    }

    /// <summary>
    /// Generates a valid ListSalesCommand with branch ID filter.
    /// </summary>
    /// <param name="branchId">The branch ID to filter by.</param>
    /// <returns>A valid ListSalesCommand with branch ID filter.</returns>
    public static ListSalesCommand GenerateCommandWithBranchFilter(Guid branchId)
    {
        return new ListSalesCommand { BranchId = branchId };
    }

    /// <summary>
    /// Generates a valid ListSalesCommand with date range filter.
    /// </summary>
    /// <param name="startDate">The start date to filter by.</param>
    /// <param name="endDate">The end date to filter by.</param>
    /// <returns>A valid ListSalesCommand with date range filter.</returns>
    public static ListSalesCommand GenerateCommandWithDateRangeFilter(DateTime startDate, DateTime endDate)
    {
        return new ListSalesCommand { StartDate = startDate, EndDate = endDate };
    }

    /// <summary>
    /// Generates a valid ListSalesCommand with all filters.
    /// </summary>
    /// <param name="customerId">The customer ID to filter by.</param>
    /// <param name="branchId">The branch ID to filter by.</param>
    /// <param name="startDate">The start date to filter by.</param>
    /// <param name="endDate">The end date to filter by.</param>
    /// <returns>A valid ListSalesCommand with all filters.</returns>
    public static ListSalesCommand GenerateCommandWithAllFilters(
        Guid customerId, 
        Guid branchId, 
        DateTime startDate, 
        DateTime endDate)
    {
        return new ListSalesCommand
        {
            CustomerId = customerId,
            BranchId = branchId,
            StartDate = startDate,
            EndDate = endDate
        };
    }

    /// <summary>
    /// Generates an invalid ListSalesCommand with end date before start date.
    /// </summary>
    /// <returns>An invalid ListSalesCommand with end date before start date.</returns>
    public static ListSalesCommand GenerateInvalidCommandWithEndDateBeforeStartDate()
    {
        return new ListSalesCommand
        {
            StartDate = DateTime.UtcNow.AddDays(1),
            EndDate = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Generates a collection of sample Sale entities for testing.
    /// </summary>
    /// <param name="count">The number of sales to generate.</param>
    /// <returns>A collection of sample Sale entities.</returns>
    public static IEnumerable<Sale> GenerateSampleSales(int count)
    {
        var faker = new Faker();
        var sales = new List<Sale>();
        
        for (int i = 0; i < count; i++)
        {
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
            for (int j = 0; j < faker.Random.Int(1, 3); j++)
            {
                sale.AddItem(
                    Guid.NewGuid(),
                    faker.Commerce.ProductName(),
                    faker.Random.Int(1, 20),
                    decimal.Parse(faker.Commerce.Price(min: 10, max: 1000))
                );
            }

            // Use reflection to set the ID since it's a private setter
            typeof(Sale).GetProperty("Id").SetValue(sale, Guid.NewGuid());
            
            // Set the sale date to a random date within the last 30 days
            var saleDate = DateTime.UtcNow.AddDays(-faker.Random.Int(0, 30));
            typeof(Sale).GetProperty("SaleDate").SetValue(sale, saleDate);

            sales.Add(sale);
        }

        return sales;
    }

    /// <summary>
    /// Generates a collection of sample Sale entities for a specific customer.
    /// </summary>
    /// <param name="customerId">The customer ID.</param>
    /// <param name="count">The number of sales to generate.</param>
    /// <returns>A collection of sample Sale entities for the specified customer.</returns>
    public static IEnumerable<Sale> GenerateSalesForCustomer(Guid customerId, int count)
    {
        var faker = new Faker();
        var sales = new List<Sale>();
        
        for (int i = 0; i < count; i++)
        {
            var branchId = Guid.NewGuid();
            
            var sale = new Sale(
                $"SALE-{DateTime.Now:yyyyMMdd}-{faker.Random.Number(100000, 999999)}",
                customerId,
                faker.Name.FullName(),
                branchId,
                faker.Company.CompanyName()
            );

            // Add some items to the sale
            for (int j = 0; j < faker.Random.Int(1, 3); j++)
            {
                sale.AddItem(
                    Guid.NewGuid(),
                    faker.Commerce.ProductName(),
                    faker.Random.Int(1, 20),
                    decimal.Parse(faker.Commerce.Price(min: 10, max: 1000))
                );
            }

            // Use reflection to set the ID since it's a private setter
            typeof(Sale).GetProperty("Id").SetValue(sale, Guid.NewGuid());

            sales.Add(sale);
        }

        return sales;
    }

    /// <summary>
    /// Generates a collection of sample Sale entities for a specific branch.
    /// </summary>
    /// <param name="branchId">The branch ID.</param>
    /// <param name="count">The number of sales to generate.</param>
    /// <returns>A collection of sample Sale entities for the specified branch.</returns>
    public static IEnumerable<Sale> GenerateSalesForBranch(Guid branchId, int count)
    {
        var faker = new Faker();
        var sales = new List<Sale>();
        
        for (int i = 0; i < count; i++)
        {
            var customerId = Guid.NewGuid();
            
            var sale = new Sale(
                $"SALE-{DateTime.Now:yyyyMMdd}-{faker.Random.Number(100000, 999999)}",
                customerId,
                faker.Name.FullName(),
                branchId,
                faker.Company.CompanyName()
            );

            // Add some items to the sale
            for (int j = 0; j < faker.Random.Int(1, 3); j++)
            {
                sale.AddItem(
                    Guid.NewGuid(),
                    faker.Commerce.ProductName(),
                    faker.Random.Int(1, 20),
                    decimal.Parse(faker.Commerce.Price(min: 10, max: 1000))
                );
            }

            // Use reflection to set the ID since it's a private setter
            typeof(Sale).GetProperty("Id").SetValue(sale, Guid.NewGuid());

            sales.Add(sale);
        }

        return sales;
    }

    /// <summary>
    /// Generates a collection of sample Sale entities within a date range.
    /// </summary>
    /// <param name="startDate">The start date.</param>
    /// <param name="endDate">The end date.</param>
    /// <param name="count">The number of sales to generate.</param>
    /// <returns>A collection of sample Sale entities within the specified date range.</returns>
    public static IEnumerable<Sale> GenerateSalesInDateRange(DateTime startDate, DateTime endDate, int count)
    {
        var faker = new Faker();
        var sales = new List<Sale>();
        
        for (int i = 0; i < count; i++)
        {
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
            for (int j = 0; j < faker.Random.Int(1, 3); j++)
            {
                sale.AddItem(
                    Guid.NewGuid(),
                    faker.Commerce.ProductName(),
                    faker.Random.Int(1, 20),
                    decimal.Parse(faker.Commerce.Price(min: 10, max: 1000))
                );
            }

            // Use reflection to set the ID since it's a private setter
            typeof(Sale).GetProperty("Id").SetValue(sale, Guid.NewGuid());
            
            // Set the sale date to a random date within the specified range
            var range = (endDate - startDate).TotalDays;
            var saleDate = startDate.AddDays(faker.Random.Double() * range);
            typeof(Sale).GetProperty("SaleDate").SetValue(sale, saleDate);

            sales.Add(sale);
        }

        return sales;
    }
}