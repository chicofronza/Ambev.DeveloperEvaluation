using Ambev.DeveloperEvaluation.Application.Sales.ListSales;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSales
{
    /// <summary>
    /// AutoMapper profile for listing sales in the web API.
    /// </summary>
    public class ListSalesProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the ListSalesProfile class.
        /// </summary>
        public ListSalesProfile()
        {
            // Map from request to command
            CreateMap<ListSalesRequest, ListSalesCommand>();

            // Map from result to response
            CreateMap<ListSalesResult, ListSalesResponse>();
            CreateMap<SaleSummary, SaleSummaryResponse>();
        }
    }
}