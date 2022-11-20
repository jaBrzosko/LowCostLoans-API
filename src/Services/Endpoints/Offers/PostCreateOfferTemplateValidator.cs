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
        
        RuleFor(ot => ot.MaximumMoneyInSmallestUnit)
            .GreaterThanOrEqualTo(ot => ot.MinimumMoneyInSmallestUnit)
            .WithErrorCode(PostCreateOfferTemplate.ErrorCodes.MaximumMoneyHasToBeSmallerThanMinimumMoney);
        
        RuleFor(ot => ot.MinimumNumberOfInstallments)
            .GreaterThan(0)
            .WithErrorCode(PostCreateOfferTemplate.ErrorCodes.NumberOfInstallmentsHasToBePositive);
        
        RuleFor(ot => ot.MaximumNumberOfInstallments)
            .GreaterThanOrEqualTo(ot => ot.MinimumNumberOfInstallments)
            .WithErrorCode(PostCreateOfferTemplate.ErrorCodes.MaximumInstallmentsHasToBeSmallerThanMinimumInstallments);
        
        RuleFor(ot => ot.InteresetRate)
            .GreaterThanOrEqualTo(0)
            .WithErrorCode(PostCreateOfferTemplate.ErrorCodes.InterestRateHasToBeNonNegative);

    }
}