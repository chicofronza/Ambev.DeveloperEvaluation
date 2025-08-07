using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Sales.ListSales;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSales;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales
{
    /// <summary>
    /// Controller for sales operations.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class SalesController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the SalesController class.
        /// </summary>
        /// <param name="mediator">The mediator instance.</param>
        /// <param name="mapper">The AutoMapper instance.</param>
        public SalesController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Lists all sales with optional filtering.
        /// </summary>
        /// <param name="customerId">Optional customer ID to filter by.</param>
        /// <param name="branchId">Optional branch ID to filter by.</param>
        /// <param name="startDate">Optional start date to filter by.</param>
        /// <param name="endDate">Optional end date to filter by.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A list of sales matching the criteria.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponseWithData<ListSalesResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ListSales(
            [FromQuery] Guid? customerId = null,
            [FromQuery] Guid? branchId = null,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null,
            CancellationToken cancellationToken = default)
        {
            var request = new ListSalesRequest
            {
                CustomerId = customerId,
                BranchId = branchId,
                StartDate = startDate,
                EndDate = endDate
            };

            var validator = new ListSalesRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            try
            {
                var command = _mapper.Map<ListSalesCommand>(request);
                var result = await _mediator.Send(command, cancellationToken);
                var response = _mapper.Map<ListSalesResponse>(result);

                return Ok(new ApiResponseWithData<ListSalesResponse>
                {
                    Success = true,
                    Message = "Sales retrieved successfully",
                    Data = response
                });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        /// <summary>
        /// Creates a new sale.
        /// </summary>
        /// <param name="request">The request to create a sale.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The created sale.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseWithData<CreateSaleResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateSale([FromBody] CreateSaleRequest request, CancellationToken cancellationToken)
        {
            var validator = new CreateSaleRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            try
            {
                var command = _mapper.Map<CreateSaleCommand>(request);
                var result = await _mediator.Send(command, cancellationToken);
                var response = _mapper.Map<CreateSaleResponse>(result);

                return Created($"/api/sales/{result.Id}", new ApiResponseWithData<CreateSaleResponse>
                {
                    Success = true,
                    Message = "Sale created successfully",
                    Data = response
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = ex.Message
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        /// <summary>
        /// Gets a sale by ID.
        /// </summary>
        /// <param name="id">The sale ID.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The sale.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponseWithData<GetSaleResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSale([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var request = new GetSaleRequest { Id = id };
            var validator = new GetSaleRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            try
            {
                var command = _mapper.Map<GetSaleCommand>(request.Id);
                var result = await _mediator.Send(command, cancellationToken);
                var response = _mapper.Map<GetSaleResponse>(result);

                return Ok(new ApiResponseWithData<GetSaleResponse>
                {
                    Success = true,
                    Message = "Sale retrieved successfully",
                    Data = response
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        /// <summary>
        /// Updates a sale.
        /// </summary>
        /// <param name="id">The sale ID.</param>
        /// <param name="request">The request to update the sale.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The updated sale.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponseWithData<UpdateSaleResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateSale([FromRoute] Guid id, [FromBody] UpdateSaleRequest request, CancellationToken cancellationToken)
        {
            var validator = new UpdateSaleRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            try
            {
                var command = _mapper.Map<UpdateSaleCommand>(request);
                command.Id = id; // Set the ID from the route

                var result = await _mediator.Send(command, cancellationToken);
                var response = _mapper.Map<UpdateSaleResponse>(result);

                return Ok(new ApiResponseWithData<UpdateSaleResponse>
                {
                    Success = true,
                    Message = "Sale updated successfully",
                    Data = response
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = ex.Message
                });
            }
            catch (Exception ex) when (ex is ValidationException || ex.Message.Contains("Cannot"))
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        /// <summary>
        /// Deletes a sale by ID.
        /// </summary>
        /// <param name="id">The sale ID.</param>
        /// <param name="canDelete">Optional parameter to physically delete the record instead of just marking it as cancelled. Default is false.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Success response if the sale was deleted.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteSale(
            [FromRoute] Guid id, 
            [FromQuery] bool canDelete = false, 
            CancellationToken cancellationToken = default)
        {
            var request = new DeleteSaleRequest { Id = id, CanDelete = canDelete };
            var validator = new DeleteSaleRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            try
            {
                var command = _mapper.Map<DeleteSaleCommand>(request);
                await _mediator.Send(command, cancellationToken);

                string message = canDelete 
                    ? "Sale permanently deleted successfully" 
                    : "Sale cancelled successfully";

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = message
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = ex.Message
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }
    }
}