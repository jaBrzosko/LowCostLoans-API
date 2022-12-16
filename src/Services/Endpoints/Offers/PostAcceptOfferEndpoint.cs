using System.Reflection.Metadata;
using Contracts.Offers;
using Domain.Offers;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Services.Data;
using Services.Data.Repositories;
using Services.Services.BlobStorages;

namespace Services.Endpoints.Offers;

public class PostAcceptOfferEndpoint: Endpoint<PostAcceptOffer>
{
    private CoreDbContext coreDbContext;
    private readonly BlobStorage blobStorage;
    public PostAcceptOfferEndpoint(CoreDbContext coreDbContext, BlobStorage blobStorage)
    {
        this.coreDbContext = coreDbContext;
        this.blobStorage = blobStorage;
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

        await blobStorage.SaveFileToBlob(req.OfferId, req.Contract, ct);
        
        offer.Status = OfferStatus.Pending;
        coreDbContext.Set<Offer>().Update(offer);
        await coreDbContext.SaveChangesAsync(ct);
    }
}