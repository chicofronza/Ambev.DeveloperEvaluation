using Ambev.DeveloperEvaluation.Application.Sales.ListSales;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Domain;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Contains unit tests for the <see cref="ListSalesHandler"/> class.
/// </summary>
public class ListSalesHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ListSalesHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="ListSalesHandlerTests"/> class.
    /// Sets up the test dependencies and creates fake data generators.
    /// </summary>
    public ListSalesHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new ListSalesHandler(_saleRepository, _mapper);
    }

    /// <summary>
    /// Tests that listing sales with no filters returns all sales.
    /// </summary>
    [Fact(DisplayName = "Given no filters When listing sales Then returns all sales")]
    public async Task Handle_NoFilters_ReturnsAllSales()
    {
        // Given
        var command = ListSalesHandlerTestData.GenerateValidCommand();
        var sales = ListSalesHandlerTestData.GenerateSampleSales(5).ToList();
        
        _saleRepository.ListAsync(
            Arg.Any<Guid?>(), 
            Arg.Any<Guid?>(), 
            Arg.Any<DateTime?>(), 
            Arg.Any<DateTime?>(), 
            Arg.Any<CancellationToken>())
            .Returns(sales);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Sales.Should().HaveCount(sales.Count);
        
        await _saleRepository.Received(1).ListAsync(
            Arg.Is<Guid?>(id => id == null),
            Arg.Is<Guid?>(id => id == null),
            Arg.Is<DateTime?>(date => date == null),
            Arg.Is<DateTime?>(date => date == null),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that listing sales with customer filter returns only sales for that customer.
    /// </summary>
    [Fact(DisplayName = "Given customer filter When listing sales Then returns sales for that customer")]
    public async Task Handle_CustomerFilter_ReturnsSalesForCustomer()
    {
        // Given
        var customerId = Guid.NewGuid();
        var command = ListSalesHandlerTestData.GenerateCommandWithCustomerFilter(customerId);
        var sales = ListSalesHandlerTestData.GenerateSalesForCustomer(customerId, 3).ToList();
        
        _saleRepository.ListAsync(
            Arg.Is<Guid?>(id => id == customerId),
            Arg.Any<Guid?>(),
            Arg.Any<DateTime?>(),
            Arg.Any<DateTime?>(),
            Arg.Any<CancellationToken>())
            .Returns(sales);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Sales.Should().HaveCount(sales.Count);
        result.Sales.Should().AllSatisfy(s => s.CustomerId.Should().Be(customerId));
        
        await _saleRepository.Received(1).ListAsync(
            Arg.Is<Guid?>(id => id == customerId),
            Arg.Is<Guid?>(id => id == null),
            Arg.Is<DateTime?>(date => date == null),
            Arg.Is<DateTime?>(date => date == null),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that listing sales with branch filter returns only sales for that branch.
    /// </summary>
    [Fact(DisplayName = "Given branch filter When listing sales Then returns sales for that branch")]
    public async Task Handle_BranchFilter_ReturnsSalesForBranch()
    {
        // Given
        var branchId = Guid.NewGuid();
        var command = ListSalesHandlerTestData.GenerateCommandWithBranchFilter(branchId);
        var sales = ListSalesHandlerTestData.GenerateSalesForBranch(branchId, 3).ToList();
        
        _saleRepository.ListAsync(
            Arg.Any<Guid?>(),
            Arg.Is<Guid?>(id => id == branchId),
            Arg.Any<DateTime?>(),
            Arg.Any<DateTime?>(),
            Arg.Any<CancellationToken>())
            .Returns(sales);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Sales.Should().HaveCount(sales.Count);
        result.Sales.Should().AllSatisfy(s => s.BranchId.Should().Be(branchId));
        
        await _saleRepository.Received(1).ListAsync(
            Arg.Is<Guid?>(id => id == null),
            Arg.Is<Guid?>(id => id == branchId),
            Arg.Is<DateTime?>(date => date == null),
            Arg.Is<DateTime?>(date => date == null),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that listing sales with date range filter returns only sales within that date range.
    /// </summary>
    [Fact(DisplayName = "Given date range filter When listing sales Then returns sales within that date range")]
    public async Task Handle_DateRangeFilter_ReturnsSalesInDateRange()
    {
        // Given
        var startDate = DateTime.UtcNow.AddDays(-10);
        var endDate = DateTime.UtcNow;
        var command = ListSalesHandlerTestData.GenerateCommandWithDateRangeFilter(startDate, endDate);
        var sales = ListSalesHandlerTestData.GenerateSalesInDateRange(startDate, endDate, 3).ToList();
        
        _saleRepository.ListAsync(
            Arg.Any<Guid?>(),
            Arg.Any<Guid?>(),
            Arg.Is<DateTime?>(date => date == startDate),
            Arg.Is<DateTime?>(date => date == endDate),
            Arg.Any<CancellationToken>())
            .Returns(sales);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Sales.Should().HaveCount(sales.Count);
        
        await _saleRepository.Received(1).ListAsync(
            Arg.Is<Guid?>(id => id == null),
            Arg.Is<Guid?>(id => id == null),
            Arg.Is<DateTime?>(date => date == startDate),
            Arg.Is<DateTime?>(date => date == endDate),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that listing sales with all filters returns only sales matching all criteria.
    /// </summary>
    [Fact(DisplayName = "Given all filters When listing sales Then returns sales matching all criteria")]
    public async Task Handle_AllFilters_ReturnsSalesMatchingAllCriteria()
    {
        // Given
        var customerId = Guid.NewGuid();
        var branchId = Guid.NewGuid();
        var startDate = DateTime.UtcNow.AddDays(-10);
        var endDate = DateTime.UtcNow;
        var command = ListSalesHandlerTestData.GenerateCommandWithAllFilters(customerId, branchId, startDate, endDate);
        var sales = new List<Sale> { ListSalesHandlerTestData.GenerateSalesForCustomer(customerId, 1).First() };
        
        // Set the branch ID and sale date for the test sale
        var sale = sales.First();
        typeof(Sale).GetProperty("BranchId").SetValue(sale, branchId);
        typeof(Sale).GetProperty("SaleDate").SetValue(sale, startDate.AddDays(1));
        
        _saleRepository.ListAsync(
            Arg.Is<Guid?>(id => id == customerId),
            Arg.Is<Guid?>(id => id == branchId),
            Arg.Is<DateTime?>(date => date == startDate),
            Arg.Is<DateTime?>(date => date == endDate),
            Arg.Any<CancellationToken>())
            .Returns(sales);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Sales.Should().HaveCount(1);
        result.Sales[0].CustomerId.Should().Be(customerId);
        result.Sales[0].BranchId.Should().Be(branchId);
        
        await _saleRepository.Received(1).ListAsync(
            Arg.Is<Guid?>(id => id == customerId),
            Arg.Is<Guid?>(id => id == branchId),
            Arg.Is<DateTime?>(date => date == startDate),
            Arg.Is<DateTime?>(date => date == endDate),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that listing sales with invalid date range throws a validation exception.
    /// </summary>
    [Fact(DisplayName = "Given invalid date range When listing sales Then throws validation exception")]
    public async Task Handle_InvalidDateRange_ThrowsValidationException()
    {
        // Given
        var command = ListSalesHandlerTestData.GenerateInvalidCommandWithEndDateBeforeStartDate();

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    /// <summary>
    /// Tests that the result includes the correct item count for each sale.
    /// </summary>
    [Fact(DisplayName = "Given sales with items When listing sales Then returns correct item count")]
    public async Task Handle_SalesWithItems_ReturnsCorrectItemCount()
    {
        // Given
        var command = ListSalesHandlerTestData.GenerateValidCommand();
        var sales = ListSalesHandlerTestData.GenerateSampleSales(3).ToList();
        
        // Cancel one item in the first sale to test the item count logic
        var firstSale = sales.First();
        var firstItem = firstSale.Items.First();
        typeof(SaleItem).GetMethod("Cancel").Invoke(firstItem, null);
        
        _saleRepository.ListAsync(
            Arg.Any<Guid?>(),
            Arg.Any<Guid?>(),
            Arg.Any<DateTime?>(),
            Arg.Any<DateTime?>(),
            Arg.Any<CancellationToken>())
            .Returns(sales);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Sales.Should().HaveCount(sales.Count);
        
        // Check that the item count for the first sale excludes the cancelled item
        var firstSaleSummary = result.Sales.First(s => s.Id == firstSale.Id);
        firstSaleSummary.ItemCount.Should().Be(firstSale.Items.Count(i => !i.IsCancelled));
    }

    /// <summary>
    /// Tests that an empty result is returned when no sales match the criteria.
    /// </summary>
    [Fact(DisplayName = "Given no matching sales When listing sales Then returns empty result")]
    public async Task Handle_NoMatchingSales_ReturnsEmptyResult()
    {
        // Given
        var command = ListSalesHandlerTestData.GenerateValidCommand();
        
        _saleRepository.ListAsync(
            Arg.Any<Guid?>(),
            Arg.Any<Guid?>(),
            Arg.Any<DateTime?>(),
            Arg.Any<DateTime?>(),
            Arg.Any<CancellationToken>())
            .Returns(new List<Sale>());

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Sales.Should().BeEmpty();
    }
}