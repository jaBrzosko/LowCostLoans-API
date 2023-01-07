using Domain.Offers;
using FastEndpoints;
using Contracts.Offers;
using Services.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Services.Data;

namespace Services.Endpoints.Offers;

[HttpPost("/offers/decide")]
[AllowAnonymous]
public class PostOfferDecideEndpoint : Endpoint<PostOfferDecide>
{
    private readonly CoreDbContext coreDbContext;

    public PostOfferDecideEndpoint(CoreDbContext coreDbContext)
    {
        this.coreDbContext = coreDbContext;
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