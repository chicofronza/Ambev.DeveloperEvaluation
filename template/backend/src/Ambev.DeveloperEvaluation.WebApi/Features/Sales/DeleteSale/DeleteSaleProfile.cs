using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.DeleteSale
{
    /// <summary>
    /// AutoMapper profile for deleting a sale in the web API.
    /// </summary>
    public class DeleteSaleProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the DeleteSaleProfile class.
        /// </summary>
        public DeleteSaleProfile()
        {
            CreateMap<DeleteSaleRequest, DeleteSaleCommand>()
                .ConstructUsing(req => new DeleteSaleCommand(req.Id, req.CanDelete));
        }
    }
}