using Contracts.Offers;
using FastEndpoints;
using FluentValidation;
using Services.Data;

namespace Services.Endpoints.Offers;

public class PostOfferDecideValidator: Validator <PostOfferDecide>
{
    public PostOfferDecideValidator()
    {
        RuleFor(req => req.Id)
            .Must(GuidCheck)
            .WithErrorCode(PostOfferDecide.ErrorCodes.OfferDoesNotExist.ToString());
    }

    private bool GuidCheck(Guid offerId)
    {
        var dbContext = Resolve<CoreDbContext>();
        var offer = dbContext.Offers.Find(offerId);
        return offer != null;
    }
}