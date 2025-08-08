using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Domain;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Contains unit tests for the <see cref="GetSaleHandler"/> class.
/// </summary>
public class GetSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly GetSaleHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetSaleHandlerTests"/> class.
    /// Sets up the test dependencies and creates fake data generators.
    /// </summary>
    public GetSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetSaleHandler(_saleRepository, _mapper);
    }

    /// <summary>
    /// Tests that a valid request to get a sale returns the correct sale.
    /// </summary>
    [Fact(DisplayName = "Given valid sale ID When getting sale Then returns correct sale")]
    public async Task Handle_ValidRequest_ReturnsCorrectSale()
    {
        // Given
        var existingSale = GetSaleHandlerTestData.GenerateSampleSale();
        var command = GetSaleHandlerTestData.GenerateValidCommandWithId(existingSale.Id);
        var expectedResult = GetSaleHandlerTestData.GenerateGetSaleResult(existingSale);
        
        _saleRepository.GetByIdAsync(existingSale.Id, Arg.Any<CancellationToken>())
            .Returns(existingSale);
        _mapper.Map<GetSaleResult>(existingSale)
            .Returns(expectedResult);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Id.Should().Be(existingSale.Id);
        result.SaleNumber.Should().Be(existingSale.SaleNumber);
        result.CustomerId.Should().Be(existingSale.CustomerId);
        result.CustomerName.Should().Be(existingSale.CustomerName);
        result.BranchId.Should().Be(existingSale.BranchId);
        result.BranchName.Should().Be(existingSale.BranchName);
        result.TotalAmount.Should().Be(existingSale.TotalAmount);
        result.Status.Should().Be(existingSale.Status);
        
        await _saleRepository.Received(1).GetByIdAsync(existingSale.Id, Arg.Any<CancellationToken>());
        _mapper.Received(1).Map<GetSaleResult>(existingSale);
    }

    /// <summary>
    /// Tests that getting a non-existent sale throws a KeyNotFoundException.
    /// </summary>
    [Fact(DisplayName = "Given non-existent sale ID When getting sale Then throws KeyNotFoundException")]
    public async Task Handle_NonExistentSale_ThrowsKeyNotFoundException()
    {
        // Given
        var command = GetSaleHandlerTestData.GenerateValidCommand();
        
        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .ReturnsNull();

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Sale with ID {command.Id} not found");
    }

    /// <summary>
    /// Tests that an invalid request throws a validation exception.
    /// </summary>
    [Fact(DisplayName = "Given invalid sale ID When getting sale Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Given
        var command = GetSaleHandlerTestData.GenerateInvalidCommand();

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    /// <summary>
    /// Tests that getting a cancelled sale returns the sale with cancelled status.
    /// </summary>
    [Fact(DisplayName = "Given cancelled sale ID When getting sale Then returns sale with cancelled status")]
    public async Task Handle_CancelledSale_ReturnsSaleWithCancelledStatus()
    {
        // Given
        var cancelledSale = GetSaleHandlerTestData.GenerateCancelledSale();
        var command = GetSaleHandlerTestData.GenerateValidCommandWithId(cancelledSale.Id);
        var expectedResult = GetSaleHandlerTestData.GenerateGetSaleResult(cancelledSale);
        
        _saleRepository.GetByIdAsync(cancelledSale.Id, Arg.Any<CancellationToken>())
            .Returns(cancelledSale);
        _mapper.Map<GetSaleResult>(cancelledSale)
            .Returns(expectedResult);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Status.Should().Be(cancelledSale.Status);
        result.Items.Should().AllSatisfy(item => item.IsCancelled.Should().BeTrue());
    }

    /// <summary>
    /// Tests that the result includes all sale items.
    /// </summary>
    [Fact(DisplayName = "Given sale with items When getting sale Then returns all items")]
    public async Task Handle_SaleWithItems_ReturnsAllItems()
    {
        // Given
        var existingSale = GetSaleHandlerTestData.GenerateSampleSale();
        var itemCount = existingSale.Items.Count;
        var command = GetSaleHandlerTestData.GenerateValidCommandWithId(existingSale.Id);
        var expectedResult = GetSaleHandlerTestData.GenerateGetSaleResult(existingSale);
        
        _saleRepository.GetByIdAsync(existingSale.Id, Arg.Any<CancellationToken>())
            .Returns(existingSale);
        _mapper.Map<GetSaleResult>(existingSale)
            .Returns(expectedResult);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(itemCount);
        
        // Verify that each item in the result corresponds to an item in the sale
        foreach (var item in existingSale.Items)
        {
            result.Items.Should().Contain(i => 
                i.Id == item.Id && 
                i.ProductId == item.ProductId && 
                i.ProductName == item.ProductName &&
                i.Quantity == item.Quantity &&
                i.UnitPrice == item.UnitPrice &&
                i.DiscountPercentage == item.DiscountPercentage &&
                i.TotalAmount == item.TotalAmount &&
                i.IsCancelled == item.IsCancelled);
        }
    }
}