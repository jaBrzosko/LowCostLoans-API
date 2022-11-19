using Contracts.Inquires;
using FastEndpoints;
using FluentValidation;
using Services.Endpoints.Users;
using Services.ValidationExtensions;

namespace Services.Endpoints.Inquires;

public class PostCreateInquireAsAnonymousValidator : Validator<PostCreateInquireAsAnonymous>
{
    public PostCreateInquireAsAnonymousValidator()
    {
        RuleFor(iq => iq.MoneyInSmallestUnit)
            .GreaterThan(0)
            .WithErrorCode(PostCreateInquireAsAnonymous.ErrorCodes.MoneyHasToBePositive);
        
        RuleFor(iq => iq.NumberOfInstallments)
            .GreaterThan(0)
            .WithErrorCode(PostCreateInquireAsAnonymous.ErrorCodes.NumberOfInstallmentsHasToBePositive);
        
        RuleFor(iq => iq.PersonalData)
            .SetValidator(new PersonalDataValidator());
    }
}
