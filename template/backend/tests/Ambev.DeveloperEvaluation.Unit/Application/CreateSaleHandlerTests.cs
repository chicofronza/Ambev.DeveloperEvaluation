using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Domain;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Contains unit tests for the <see cref="CreateSaleHandler"/> class.
/// </summary>
public class CreateSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly CreateSaleHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateSaleHandlerTests"/> class.
    /// Sets up the test dependencies and creates fake data generators.
    /// </summary>
    public CreateSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new CreateSaleHandler(_saleRepository, _mapper);
    }

    /// <summary>
    /// Tests that a valid sale creation request is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid sale data When creating sale Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Given
        var command = CreateSaleHandlerTestData.GenerateValidCommand();
        var sale = new Sale(
            "SALE-20230101-123456",
            command.CustomerId,
            command.CustomerName,
            command.BranchId,
            command.BranchName
        );

        // Add items to the sale
        foreach (var item in command.Items)
        {
            sale.AddItem(item.ProductId, item.ProductName, item.Quantity, item.UnitPrice);
        }

        var result = new CreateSaleResult
        {
            Id = Guid.NewGuid(),
            SaleNumber = sale.SaleNumber,
            SaleDate = sale.SaleDate,
            CustomerId = sale.CustomerId,
            CustomerName = sale.CustomerName,
            BranchId = sale.BranchId,
            BranchName = sale.BranchName,
            TotalAmount = sale.TotalAmount
        };

        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(sale);
        _mapper.Map<CreateSaleResult>(Arg.Any<Sale>()).Returns(result);

        // When
        var createSaleResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        createSaleResult.Should().NotBeNull();
        createSaleResult.Id.Should().Be(result.Id);
        createSaleResult.SaleNumber.Should().Be(result.SaleNumber);
        createSaleResult.CustomerId.Should().Be(command.CustomerId);
        createSaleResult.CustomerName.Should().Be(command.CustomerName);
        createSaleResult.BranchId.Should().Be(command.BranchId);
        createSaleResult.BranchName.Should().Be(command.BranchName);
        
        await _saleRepository.Received(1).CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that an invalid sale creation request throws a validation exception.
    /// </summary>
    [Fact(DisplayName = "Given invalid sale data When creating sale Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Given
        var command = CreateSaleHandlerTestData.GenerateInvalidCommandWithNoItems();

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    /// <summary>
    /// Tests that a sale with empty customer name throws a validation exception.
    /// </summary>
    [Fact(DisplayName = "Given sale with empty customer name When creating sale Then throws validation exception")]
    public async Task Handle_EmptyCustomerName_ThrowsValidationException()
    {
        // Given
        var command = CreateSaleHandlerTestData.GenerateInvalidCommandWithEmptyCustomerName();

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    /// <summary>
    /// Tests that a sale with excessive item quantity throws a validation exception.
    /// </summary>
    [Fact(DisplayName = "Given sale with excessive item quantity When creating sale Then throws validation exception")]
    public async Task Handle_ExcessiveQuantity_ThrowsValidationException()
    {
        // Given
        var command = CreateSaleHandlerTestData.GenerateInvalidCommandWithExcessiveQuantity();

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    /// <summary>
    /// Tests that the sale number is generated correctly.
    /// </summary>
    [Fact(DisplayName = "Given valid sale data When creating sale Then generates sale number")]
    public async Task Handle_ValidRequest_GeneratesSaleNumber()
    {
        // Given
        var command = CreateSaleHandlerTestData.GenerateValidCommand();
        
        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(callInfo => 
            {
                var sale = callInfo.Arg<Sale>();
                return sale;
            });

        _mapper.Map<CreateSaleResult>(Arg.Any<Sale>())
            .Returns(callInfo => 
            {
                var sale = callInfo.Arg<Sale>();
                return new CreateSaleResult 
                { 
                    Id = sale.Id,
                    SaleNumber = sale.SaleNumber
                };
            });

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.SaleNumber.Should().NotBeNullOrEmpty();
        result.SaleNumber.Should().StartWith("SALE-");
        result.SaleNumber.Should().MatchRegex(@"SALE-\d{8}-\d{6}");
    }

    /// <summary>
    /// Tests that the repository is called with the correct sale entity.
    /// </summary>
    [Fact(DisplayName = "Given valid sale data When creating sale Then calls repository with correct data")]
    public async Task Handle_ValidRequest_CallsRepositoryWithCorrectData()
    {
        // Given
        var command = CreateSaleHandlerTestData.GenerateValidCommand();
        
        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(callInfo => callInfo.Arg<Sale>());

        _mapper.Map<CreateSaleResult>(Arg.Any<Sale>())
            .Returns(new CreateSaleResult());

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        await _saleRepository.Received(1).CreateAsync(
            Arg.Is<Sale>(s => 
                s.CustomerId == command.CustomerId &&
                s.CustomerName == command.CustomerName &&
                s.BranchId == command.BranchId &&
                s.BranchName == command.BranchName &&
                s.Items.Count == command.Items.Count
            ), 
            Arg.Any<CancellationToken>()
        );
    }

    /// <summary>
    /// Tests that the discount is applied correctly based on quantity.
    /// </summary>
    [Fact(DisplayName = "Given sale with items eligible for discount When creating sale Then applies correct discount")]
    public async Task Handle_ItemsWithDiscount_AppliesCorrectDiscount()
    {
        // Given
        var command = CreateSaleHandlerTestData.GenerateCommandWithItemQuantities(3, 5, 12);
        
        Sale capturedSale = null;
        
        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(callInfo => 
            {
                capturedSale = callInfo.Arg<Sale>();
                return capturedSale;
            });

        _mapper.Map<CreateSaleResult>(Arg.Any<Sale>())
            .Returns(new CreateSaleResult());

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        capturedSale.Should().NotBeNull();
        var items = capturedSale.Items.ToList();
        
        // Item with quantity 3 should have no discount
        items[0].DiscountPercentage.Should().Be(0m);
        
        // Item with quantity 5 should have 10% discount
        items[1].DiscountPercentage.Should().Be(0.1m);
        
        // Item with quantity 12 should have 20% discount
        items[2].DiscountPercentage.Should().Be(0.2m);
    }
}