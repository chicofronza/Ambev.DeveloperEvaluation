using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale
{
    /// <summary>
    /// AutoMapper profile for getting a sale in the web API.
    /// </summary>
    public class GetSaleProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the GetSaleProfile class.
        /// </summary>
        public GetSaleProfile()
        {
            CreateMap<Guid, GetSaleCommand>().ConstructUsing(id => new GetSaleCommand(id));
            CreateMap<GetSaleResult, GetSaleResponse>();
            CreateMap<SaleItemResult, GetSaleItemResponse>();
        }
    }
}