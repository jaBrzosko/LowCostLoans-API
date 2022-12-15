using Contracts.Offers;
using FastEndpoints;
using FluentValidation;

namespace Services.Endpoints.Offers;

public class PostAcceptOfferValidator: Validator<PostAcceptOffer>
{
    public PostAcceptOfferValidator()
    {
        // RuleFor(req => req.Contract.Length)
        //     .LessThan(16 * 1024 * 1024)
        //     .WithErrorCode(PostAcceptOffer.ErrorCodes.FileHasToBeSmallerThan16MB.ToString());
    }
}