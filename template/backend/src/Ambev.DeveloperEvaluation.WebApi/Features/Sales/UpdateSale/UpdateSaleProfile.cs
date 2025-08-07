using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale
{
    /// <summary>
    /// AutoMapper profile for updating a sale in the web API.
    /// </summary>
    public class UpdateSaleProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the UpdateSaleProfile class.
        /// </summary>
        public UpdateSaleProfile()
        {
            // Map from request to command
            CreateMap<UpdateSaleRequest, UpdateSaleCommand>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()); // ID comes from route, not request body
            CreateMap<UpdateSaleItemRequest, UpdateSaleItemCommand>();

            // Map from result to response
            CreateMap<UpdateSaleResult, UpdateSaleResponse>();
            CreateMap<UpdateSaleItemResult, UpdateSaleItemResponse>();
        }
    }
}