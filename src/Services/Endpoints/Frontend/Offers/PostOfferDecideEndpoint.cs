using Contracts.Frontend.Offers;
using Domain.Offers;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Services.Data;
using Services.Services.AuthServices;

namespace Services.Endpoints.Frontend.Offers;

public class PostOfferDecideEndpoint : Endpoint<PostOfferDecide>
{
    private readonly CoreDbContext coreDbContext;

    public PostOfferDecideEndpoint(CoreDbContext coreDbContext)
    {
        this.coreDbContext = coreDbContext;
    }

    public override void Configure()
    {
        Post("/frontend/offers/decide");
        Roles(AuthRoles.Admin);
    }

    public override async Task HandleAsync(PostOfferDecide req, CancellationToken ct)
    {
        var offer = await coreDbContext
            .Offers
            .Where(o => o.Id == req.Id)
            .FirstOrDefaultAsync(ct);
        
        offer.Status = req.AcceptOffer ? OfferStatus.Accepted : OfferStatus.Rejected;
        
        coreDbContext.Set<Offer>().Update(offer);        
        await coreDbContext.SaveChangesAsync(ct);
    }
}