using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    /// <summary>
    /// AutoMapper profile for getting a sale.
    /// </summary>
    public class GetSaleProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the GetSaleProfile class.
        /// </summary>
        public GetSaleProfile()
        {
            CreateMap<Sale, GetSaleResult>();
            CreateMap<SaleItem, SaleItemResult>();
        }
    }
}