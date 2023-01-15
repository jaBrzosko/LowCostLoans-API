using Contracts.Offers;
using FastEndpoints;
using FluentValidation;
using Services.Data;
using Services.ValidationExtensions;

namespace Services.Endpoints.Offers;

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
        var offer = await dbContext.Offers.FindAsync(offerId);
        return offer != null;
    }
}