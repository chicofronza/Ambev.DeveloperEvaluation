using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    /// <summary>
    /// AutoMapper profile for sale creation.
    /// </summary>
    public class CreateSaleProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the CreateSaleProfile class.
        /// </summary>
        public CreateSaleProfile()
        {
            CreateMap<Sale, CreateSaleResult>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

            CreateMap<SaleItem, SaleItemResult>();
        }
    }
}