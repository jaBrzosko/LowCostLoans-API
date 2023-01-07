using Contracts.Api.Offers;
using Contracts.Shared.Offers;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Services.Data;

namespace Services.Endpoints.Offers;

[HttpGet("/offers/getOfferStatus")]
[AllowAnonymous]
public class GetOfferStatusEndpoint : Endpoint<GetOfferStatus, OfferStatusDto?>
{
    private readonly CoreDbContext dbContext;

    public GetOfferStatusEndpoint(CoreDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public override async Task HandleAsync(GetOfferStatus req, CancellationToken ct)
    {
        var result = await dbContext
            .Offers
            .Where(o => o.Id == req.Id)
            .Select(o => new OfferStatusDto
            {
                Id = o.Id,
                Status = (OfferStatusTypeDto)o.Status
            })
            .FirstOrDefaultAsync(ct);
        await SendAsync(result, cancellation: ct);
    }
}