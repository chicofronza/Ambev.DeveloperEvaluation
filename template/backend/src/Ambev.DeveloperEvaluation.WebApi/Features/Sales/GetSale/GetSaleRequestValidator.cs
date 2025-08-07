using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale
{
    /// <summary>
    /// Validator for the GetSaleRequest.
    /// </summary>
    public class GetSaleRequestValidator : AbstractValidator<GetSaleRequest>
    {
        /// <summary>
        /// Initializes a new instance of the GetSaleRequestValidator class.
        /// </summary>
        public GetSaleRequestValidator()
        {
            RuleFor(r => r.Id)
                .NotEmpty().WithMessage("Sale ID is required.");
        }
    }
}