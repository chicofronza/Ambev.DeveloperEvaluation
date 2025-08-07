using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale
{
    /// <summary>
    /// AutoMapper profile for sale creation in the web API.
    /// </summary>
    public class CreateSaleProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the CreateSaleProfile class.
        /// </summary>
        public CreateSaleProfile()
        {
            // Map from request to command
            CreateMap<CreateSaleRequest, CreateSaleCommand>();
            CreateMap<CreateSaleItemRequest, CreateSaleItemCommand>();

            // Map from result to response
            CreateMap<CreateSaleResult, CreateSaleResponse>();
            CreateMap<SaleItemResult, SaleItemResponse>();
        }
    }
}