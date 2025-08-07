using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    /// <summary>
    /// AutoMapper profile for updating a sale.
    /// </summary>
    public class UpdateSaleProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the UpdateSaleProfile class.
        /// </summary>
        public UpdateSaleProfile()
        {
            CreateMap<Sale, UpdateSaleResult>();
            CreateMap<SaleItem, UpdateSaleItemResult>();
        }
    }
}