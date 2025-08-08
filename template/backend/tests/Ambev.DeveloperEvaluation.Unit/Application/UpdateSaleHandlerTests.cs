using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
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
/// Contains unit tests for the <see cref="UpdateSaleHandler"/> class.
/// </summary>
public class UpdateSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly UpdateSaleHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateSaleHandlerTests"/> class.
    /// Sets up the test dependencies and creates fake data generators.
    /// </summary>
    public UpdateSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new UpdateSaleHandler(_saleRepository, _mapper);
    }

    /// <summary>
    /// Tests that a valid sale update request is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid sale data When updating sale Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Given
        var existingSale = UpdateSaleHandlerTestData.GenerateSampleSale();
        var command = UpdateSaleHandlerTestData.GenerateValidCommandWithId(existingSale.Id);
        
        var result = new UpdateSaleResult
        {
            Id = existingSale.Id,
            SaleNumber = existingSale.SaleNumber,
            SaleDate = existingSale.SaleDate,
            CustomerId = command.CustomerId,
            CustomerName = command.CustomerName,
            BranchId = command.BranchId,
            BranchName = command.BranchName,
            TotalAmount = existingSale.TotalAmount
        };

        _saleRepository.GetByIdAsync(existingSale.Id, Arg.Any<CancellationToken>())
            .Returns(existingSale);
        _saleRepository.UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(existingSale);
        _mapper.Map<UpdateSaleResult>(Arg.Any<Sale>()).Returns(result);

        // When
        var updateSaleResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        updateSaleResult.Should().NotBeNull();
        updateSaleResult.Id.Should().Be(existingSale.Id);
        updateSaleResult.SaleNumber.Should().Be(existingSale.SaleNumber);
        updateSaleResult.CustomerId.Should().Be(command.CustomerId);
        updateSaleResult.CustomerName.Should().Be(command.CustomerName);
        updateSaleResult.BranchId.Should().Be(command.BranchId);
        updateSaleResult.BranchName.Should().Be(command.BranchName);
        
        await _saleRepository.Received(1).GetByIdAsync(existingSale.Id, Arg.Any<CancellationToken>());
        await _saleRepository.Received(1).UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that updating a non-existent sale throws a KeyNotFoundException.
    /// </summary>
    [Fact(DisplayName = "Given non-existent sale ID When updating sale Then throws KeyNotFoundException")]
    public async Task Handle_NonExistentSale_ThrowsKeyNotFoundException()
    {
        // Given
        var command = UpdateSaleHandlerTestData.GenerateValidCommand();
        
        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .ReturnsNull();

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Sale with ID {command.Id} not found");
    }

    /// <summary>
    /// Tests that an invalid sale update request throws a validation exception.
    /// </summary>
    [Fact(DisplayName = "Given invalid sale data When updating sale Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Given
        var command = UpdateSaleHandlerTestData.GenerateInvalidCommandWithNoItems();

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    /// <summary>
    /// Tests that a sale with empty customer name throws a validation exception.
    /// </summary>
    [Fact(DisplayName = "Given sale with empty customer name When updating sale Then throws validation exception")]
    public async Task Handle_EmptyCustomerName_ThrowsValidationException()
    {
        // Given
        var command = UpdateSaleHandlerTestData.GenerateInvalidCommandWithEmptyCustomerName();

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    /// <summary>
    /// Tests that a sale with excessive item quantity throws a validation exception.
    /// </summary>
    [Fact(DisplayName = "Given sale with excessive item quantity When updating sale Then throws validation exception")]
    public async Task Handle_ExcessiveQuantity_ThrowsValidationException()
    {
        // Given
        var command = UpdateSaleHandlerTestData.GenerateInvalidCommandWithExcessiveQuantity();

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    /// <summary>
    /// Tests that the customer and branch information is updated correctly.
    /// </summary>
    [Fact(DisplayName = "Given valid sale data When updating sale Then updates customer and branch info")]
    public async Task Handle_ValidRequest_UpdatesCustomerAndBranchInfo()
    {
        // Given
        var existingSale = UpdateSaleHandlerTestData.GenerateSampleSale();
        var command = UpdateSaleHandlerTestData.GenerateValidCommandWithId(existingSale.Id);
        
        Sale capturedSale = null;
        
        _saleRepository.GetByIdAsync(existingSale.Id, Arg.Any<CancellationToken>())
            .Returns(existingSale);
        _saleRepository.UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(callInfo => 
            {
                capturedSale = callInfo.Arg<Sale>();
                return capturedSale;
            });
        _mapper.Map<UpdateSaleResult>(Arg.Any<Sale>())
            .Returns(new UpdateSaleResult());

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        capturedSale.Should().NotBeNull();
        capturedSale.CustomerId.Should().Be(command.CustomerId);
        capturedSale.CustomerName.Should().Be(command.CustomerName);
        capturedSale.BranchId.Should().Be(command.BranchId);
        capturedSale.BranchName.Should().Be(command.BranchName);
    }

    /// <summary>
    /// Tests that existing items are updated correctly.
    /// </summary>
    [Fact(DisplayName = "Given existing items When updating sale Then updates existing items")]
    public async Task Handle_ExistingItems_UpdatesExistingItems()
    {
        // Given
        var existingSale = UpdateSaleHandlerTestData.GenerateSampleSale();
        var existingItem = existingSale.Items.First();
        
        var command = UpdateSaleHandlerTestData.GenerateValidCommandWithId(existingSale.Id);
        command.Items.Clear();
        command.Items.Add(new UpdateSaleItemCommand
        {
            ProductId = existingItem.ProductId,
            ProductName = "Updated Product Name",
            Quantity = 10, // Change quantity to trigger update
            UnitPrice = existingItem.UnitPrice
        });
        
        Sale capturedSale = null;
        
        _saleRepository.GetByIdAsync(existingSale.Id, Arg.Any<CancellationToken>())
            .Returns(existingSale);
        _saleRepository.UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(callInfo => 
            {
                capturedSale = callInfo.Arg<Sale>();
                return capturedSale;
            });
        _mapper.Map<UpdateSaleResult>(Arg.Any<Sale>())
            .Returns(new UpdateSaleResult());

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        capturedSale.Should().NotBeNull();
        var updatedItem = capturedSale.Items.First(i => i.ProductId == existingItem.ProductId);
        updatedItem.Should().NotBeNull();
        updatedItem.Quantity.Should().Be(10);
        
        // Verify that UpdateItem was called with the correct parameters
        // This is indirect since we can't directly verify private method calls
        updatedItem.Quantity.Should().Be(10);
    }

    /// <summary>
    /// Tests that new items are added correctly.
    /// </summary>
    [Fact(DisplayName = "Given new items When updating sale Then adds new items")]
    public async Task Handle_NewItems_AddsNewItems()
    {
        // Given
        var existingSale = UpdateSaleHandlerTestData.GenerateSampleSale();
        var initialItemCount = existingSale.Items.Count;
        
        var command = UpdateSaleHandlerTestData.GenerateValidCommandWithId(existingSale.Id);
        var newItem = new UpdateSaleItemCommand
        {
            ProductId = Guid.NewGuid(), // New product ID not in existing sale
            ProductName = "New Product",
            Quantity = 5,
            UnitPrice = 100m
        };
        command.Items.Clear();
        command.Items.Add(newItem);
        
        Sale capturedSale = null;
        
        _saleRepository.GetByIdAsync(existingSale.Id, Arg.Any<CancellationToken>())
            .Returns(existingSale);
        _saleRepository.UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(callInfo => 
            {
                capturedSale = callInfo.Arg<Sale>();
                return capturedSale;
            });
        _mapper.Map<UpdateSaleResult>(Arg.Any<Sale>())
            .Returns(new UpdateSaleResult());

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        capturedSale.Should().NotBeNull();
        capturedSale.Items.Count.Should().Be(initialItemCount + 1);
        var addedItem = capturedSale.Items.FirstOrDefault(i => i.ProductId == newItem.ProductId);
        addedItem.Should().NotBeNull();
        addedItem.ProductName.Should().Be(newItem.ProductName);
        addedItem.Quantity.Should().Be(newItem.Quantity);
        addedItem.UnitPrice.Should().Be(newItem.UnitPrice);
    }

    /// <summary>
    /// Tests that the discount is applied correctly based on quantity.
    /// </summary>
    [Fact(DisplayName = "Given items with different quantities When updating sale Then applies correct discounts")]
    public async Task Handle_ItemsWithDifferentQuantities_AppliesCorrectDiscounts()
    {
        // Given
        var existingSale = UpdateSaleHandlerTestData.GenerateSampleSale();
        
        var command = UpdateSaleHandlerTestData.GenerateCommandWithItemQuantities(3, 5, 12);
        command.Id = existingSale.Id;
        
        Sale capturedSale = null;
        
        _saleRepository.GetByIdAsync(existingSale.Id, Arg.Any<CancellationToken>())
            .Returns(existingSale);
        _saleRepository.UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(callInfo => 
            {
                capturedSale = callInfo.Arg<Sale>();
                return capturedSale;
            });
        _mapper.Map<UpdateSaleResult>(Arg.Any<Sale>())
            .Returns(new UpdateSaleResult());

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        capturedSale.Should().NotBeNull();
        
        // Find the items by their quantities
        var item3 = capturedSale.Items.FirstOrDefault(i => i.Quantity == 3);
        var item5 = capturedSale.Items.FirstOrDefault(i => i.Quantity == 5);
        var item12 = capturedSale.Items.FirstOrDefault(i => i.Quantity == 12);
        
        // Item with quantity 3 should have no discount
        item3.Should().NotBeNull();
        item3.DiscountPercentage.Should().Be(0m);
        
        // Item with quantity 5 should have 10% discount
        item5.Should().NotBeNull();
        item5.DiscountPercentage.Should().Be(0.1m);
        
        // Item with quantity 12 should have 20% discount
        item12.Should().NotBeNull();
        item12.DiscountPercentage.Should().Be(0.2m);
    }
}