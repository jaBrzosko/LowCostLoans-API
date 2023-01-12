using Contracts.Api.Inquiries;
using FastEndpoints;
using FluentValidation;
using Services.Endpoints.Users;
using Services.ValidationExtensions;

namespace Services.Endpoints.Api.Inquiries;

public class PostCreateAnonymousInquireValidator : Validator<PostCreateAnonymousInquire>
{
    public PostCreateAnonymousInquireValidator()
    {
        RuleFor(iq => iq.MoneyInSmallestUnit)
            .GreaterThan(0)
            .WithErrorCode(PostCreateAnonymousInquire.ErrorCodes.MoneyHasToBePositive);

        RuleFor(iq => iq.NumberOfInstallments)
            .GreaterThan(0)
            .WithErrorCode(PostCreateAnonymousInquire.ErrorCodes.NumberOfInstallmentsHasToBePositive);

        RuleFor(iq => iq.PersonalData)
            .SetValidator(new PersonalDataValidator());
    }
}
