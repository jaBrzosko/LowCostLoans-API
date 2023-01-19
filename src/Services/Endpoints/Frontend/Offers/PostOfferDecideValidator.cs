using Contracts.Frontend.Offers;
using FastEndpoints;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Services.Data;
using Services.ValidationExtensions;

namespace Services.Endpoints.Frontend.Offers;

public class PostOfferDecideValidator: Validator <PostOfferDecide>
{
    public PostOfferDecideValidator()
    {
        RuleFor(req => req.Id)
            .MustAsync(DoesOfferExistAsync)
            .WithErrorCode(PostOfferDecide.ErrorCodes.OfferDoesNotExist);
    }

    private async Task<bool> DoesOfferExistAsync(Guid offerId, CancellationToken ct)
    {
        var dbContext = Resolve<CoreDbContext>();
        var offer = await dbContext.Offers.FirstOrDefaultAsync(o => o.Id == offerId, ct);
        return offer != null;
    }
}