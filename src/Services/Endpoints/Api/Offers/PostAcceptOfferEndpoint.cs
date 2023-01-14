using Contracts.Api.Offers;
using Domain.Offers;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Services.Data;
using Services.Services.BlobStorages;

namespace Services.Endpoints.Api.Offers;

public class PostAcceptOfferEndpoint: Endpoint<PostAcceptOffer>
{
    private readonly CoreDbContext coreDbContext;
    private readonly BlobStorage blobStorage;
    
    public PostAcceptOfferEndpoint(CoreDbContext coreDbContext, BlobStorage blobStorage)
    {
        this.coreDbContext = coreDbContext;
        this.blobStorage = blobStorage;
    }

    public override void Configure()
    {
        AllowFileUploads();
        Post("/api/offers/accept");
    }

    public override async Task HandleAsync(PostAcceptOffer req, CancellationToken ct)
    {
        // TODO: do this with repository
        var offer = await coreDbContext
            .Offers
            .FirstOrDefaultAsync(o => o.Id == req.OfferId, ct);

        await blobStorage.SaveFileToBlob(req.OfferId, req.Contract, ct);
        
        offer.Status = OfferStatus.Pending;
        coreDbContext.Offers.Update(offer);
        await coreDbContext.SaveChangesAsync(ct);
    }
}
