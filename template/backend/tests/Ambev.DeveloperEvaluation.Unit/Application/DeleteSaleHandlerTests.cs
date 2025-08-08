using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Domain;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Contains unit tests for the <see cref="DeleteSaleHandler"/> class.
/// </summary>
public class DeleteSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly DeleteSaleHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteSaleHandlerTests"/> class.
    /// Sets up the test dependencies and creates fake data generators.
    /// </summary>
    public DeleteSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _handler = new DeleteSaleHandler(_saleRepository);
    }

    /// <summary>
    /// Tests that a valid sale soft deletion request is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid sale ID When soft deleting sale Then marks sale as cancelled")]
    public async Task Handle_ValidRequestSoftDelete_MarksSaleAsCancelled()
    {
        // Given
        var existingSale = DeleteSaleHandlerTestData.GenerateSampleSale();
        var command = DeleteSaleHandlerTestData.GenerateValidCommandWithId(existingSale.Id);
        
        Sale capturedSale = null;
        
        _saleRepository.GetByIdAsync(existingSale.Id, Arg.Any<CancellationToken>())
            .Returns(existingSale);
        _saleRepository.UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(callInfo => 
            {
                capturedSale = callInfo.Arg<Sale>();
                return capturedSale;
            });

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        
        capturedSale.Should().NotBeNull();
        capturedSale.Status.Should().Be(SaleStatus.Cancelled);
        
        await _saleRepository.Received(1).GetByIdAsync(existingSale.Id, Arg.Any<CancellationToken>());
        await _saleRepository.Received(1).UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
        await _saleRepository.DidNotReceive().DeleteAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that a valid sale physical deletion request is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid sale ID When physically deleting sale Then deletes sale from repository")]
    public async Task Handle_ValidRequestPhysicalDelete_DeletesSaleFromRepository()
    {
        // Given
        var existingSale = DeleteSaleHandlerTestData.GenerateSampleSale();
        var command = DeleteSaleHandlerTestData.GenerateValidCommandWithPhysicalDeletion(existingSale.Id);
        
        _saleRepository.GetByIdAsync(existingSale.Id, Arg.Any<CancellationToken>())
            .Returns(existingSale);
        _saleRepository.DeleteAsync(existingSale.Id, Arg.Any<CancellationToken>())
            .Returns(true);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        
        await _saleRepository.Received(1).GetByIdAsync(existingSale.Id, Arg.Any<CancellationToken>());
        await _saleRepository.Received(1).DeleteAsync(existingSale.Id, Arg.Any<CancellationToken>());
        await _saleRepository.DidNotReceive().UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that deleting a non-existent sale throws a KeyNotFoundException.
    /// </summary>
    [Fact(DisplayName = "Given non-existent sale ID When deleting sale Then throws KeyNotFoundException")]
    public async Task Handle_NonExistentSale_ThrowsKeyNotFoundException()
    {
        // Given
        var command = DeleteSaleHandlerTestData.GenerateValidCommand();
        
        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
            .ReturnsNull();

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Sale with ID {command.Id} not found");
    }

    /// <summary>
    /// Tests that a failed physical deletion throws an InvalidOperationException.
    /// </summary>
    [Fact(DisplayName = "Given repository failure When physically deleting sale Then throws InvalidOperationException")]
    public async Task Handle_FailedPhysicalDelete_ThrowsInvalidOperationException()
    {
        // Given
        var existingSale = DeleteSaleHandlerTestData.GenerateSampleSale();
        var command = DeleteSaleHandlerTestData.GenerateValidCommandWithPhysicalDeletion(existingSale.Id);
        
        _saleRepository.GetByIdAsync(existingSale.Id, Arg.Any<CancellationToken>())
            .Returns(existingSale);
        _saleRepository.DeleteAsync(existingSale.Id, Arg.Any<CancellationToken>())
            .Returns(false);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"Failed to delete sale with ID {existingSale.Id}");
    }

    /// <summary>
    /// Tests that attempting to cancel an already cancelled sale throws a DomainException.
    /// </summary>
    [Fact(DisplayName = "Given already cancelled sale When soft deleting sale Then throws DomainException")]
    public async Task Handle_AlreadyCancelledSale_ThrowsDomainException()
    {
        // Given
        var existingSale = DeleteSaleHandlerTestData.GenerateCancelledSale();
        var command = DeleteSaleHandlerTestData.GenerateValidCommandWithId(existingSale.Id);
        
        _saleRepository.GetByIdAsync(existingSale.Id, Arg.Any<CancellationToken>())
            .Returns(existingSale);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Sale is already cancelled");
    }

    /// <summary>
    /// Tests that an invalid sale deletion request throws a validation exception.
    /// </summary>
    [Fact(DisplayName = "Given invalid sale ID When deleting sale Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Given
        var command = new DeleteSaleCommand(Guid.Empty);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    /// <summary>
    /// Tests that soft deleting a sale cancels all its items.
    /// </summary>
    [Fact(DisplayName = "Given sale with items When soft deleting sale Then cancels all items")]
    public async Task Handle_SaleWithItems_CancelsAllItems()
    {
        // Given
        var existingSale = DeleteSaleHandlerTestData.GenerateSampleSale();
        var initialItemCount = existingSale.Items.Count;
        var command = DeleteSaleHandlerTestData.GenerateValidCommandWithId(existingSale.Id);
        
        Sale capturedSale = null;
        
        _saleRepository.GetByIdAsync(existingSale.Id, Arg.Any<CancellationToken>())
            .Returns(existingSale);
        _saleRepository.UpdateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(callInfo => 
            {
                capturedSale = callInfo.Arg<Sale>();
                return capturedSale;
            });

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        capturedSale.Should().NotBeNull();
        capturedSale.Items.Count.Should().Be(initialItemCount);
        capturedSale.Items.Should().AllSatisfy(item => item.IsCancelled.Should().BeTrue());
    }
}