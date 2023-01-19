using Contracts.Api.Offers;
using Domain.Offers;
using FastEndpoints;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Services.Data;
using Services.ValidationExtensions;

namespace Services.Endpoints.Frontend.Offers;

public class GetOfferDetailsValidator: Validator<GetOfferDetails>
{
    public GetOfferDetailsValidator()
    {
        RuleFor(req => req.OfferId)
            .MustAsync(DoesOfferExistAsync)
            .WithErrorCode(GetOfferDetails.ErrorCodes.OfferDoesNotExist);
        RuleFor(req => req.OfferId)
            .MustAsync(IsContractUploaded)
            .WithErrorCode(GetOfferDetails.ErrorCodes.ContractNotUploaded);
    }

    private async Task<bool> DoesOfferExistAsync(Guid offerId, CancellationToken ct)
    {
        var dbContext = Resolve<CoreDbContext>();
        var offer = await dbContext.Offers.FindAsync(offerId);
        return offer != null;
    }

    private async Task<bool> IsContractUploaded(Guid offerId, CancellationToken ct)
    {
        var dbContext = Resolve<CoreDbContext>();
        var offer = await dbContext.Offers.FirstOrDefaultAsync(x => x.Id ==offerId, ct);
        return offer.Status != OfferStatus.Created;
    }
}