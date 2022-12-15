using Contracts.Offers;
using Domain.Offers;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Services.Data;
using Services.Data.Repositories;

namespace Services.Endpoints.Offers;

public class PostAcceptOfferEndpoint: Endpoint<PostAcceptOffer>
{
    private CoreDbContext coreDbContext;
    public PostAcceptOfferEndpoint(CoreDbContext coreDbContext)
    {
        this.coreDbContext = coreDbContext;
    }

    public override void Configure()
    {
        AllowFileUploads();
        AllowAnonymous();
        Post("/offers/accept");
    }

    public override async Task HandleAsync(PostAcceptOffer req, CancellationToken ct)
    {
        var offer = await coreDbContext
            .Offers
            .FirstOrDefaultAsync(o => o.Id == req.OfferId, ct);

        offer.Status = OfferStatus.Pending;

        coreDbContext.Set<Offer>().Update(offer);
        await coreDbContext.SaveChangesAsync(ct);
    }
}