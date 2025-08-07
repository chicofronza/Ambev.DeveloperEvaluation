using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales
{
    /// <summary>
    /// Command for listing sales with optional filtering.
    /// </summary>
    public class ListSalesCommand : IRequest<ListSalesResult>
    {
        /// <summary>
        /// Gets or sets the optional customer ID to filter by.
        /// </summary>
        public Guid? CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the optional branch ID to filter by.
        /// </summary>
        public Guid? BranchId { get; set; }

        /// <summary>
        /// Gets or sets the optional start date to filter by.
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Gets or sets the optional end date to filter by.
        /// </summary>
        public DateTime? EndDate { get; set; }
    }
}