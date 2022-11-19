using Contracts.Offers;
using FastEndpoints;
using FluentValidation;
using Services.ValidationExtensions;

namespace Services.Endpoints.Offers;

public class PostCreateOfferTemplateValidator: Validator<PostCreateOfferTemplate>
{
    public PostCreateOfferTemplateValidator()
    {
        RuleFor(ot => ot.MinimumMoneyInSmallestUnit)
            .GreaterThan(0)
            .WithErrorCode(PostCreateOfferTemplate.ErrorCodes.MoneyHasToBePositive);
        
        RuleFor(ot => ot.MinimumNumberOfInstallments)
            .GreaterThan(0)
            .WithErrorCode(PostCreateOfferTemplate.ErrorCodes.NumberOfInstallmentsHasToBePositive);
        
        RuleFor(ot => ot.InteresetRate)
            .GreaterThanOrEqualTo(0)
            .WithErrorCode(PostCreateOfferTemplate.ErrorCodes.InterestRateHasToBeNonNegative);
    }
}