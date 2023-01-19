using Contracts.Api.Offers;
using FastEndpoints;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Services.Data;
using Services.ValidationExtensions;

namespace Services.Endpoints.Api.Offers;

public class PostAcceptOfferValidator : Validator<PostAcceptOffer>
{
    public PostAcceptOfferValidator()
    {
        RuleFor(req => req.OfferId)
            .MustAsync(DoesOfferExist)
            .WithErrorCode(PostAcceptOffer.ErrorCodes.OfferDoesNotExist);
    }

    private Task<bool> DoesOfferExist(Guid offerId, CancellationToken ct)
    {
        return Resolve<CoreDbContext>()
            .Offers
            .AnyAsync(o => o.Id == offerId, ct);
    }
}
