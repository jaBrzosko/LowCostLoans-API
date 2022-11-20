using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Services.Data;
using Contracts.Offers;

namespace Services.Endpoints.Offers;

[HttpPost("/offers/getOfferSByInquireId")]
[AllowAnonymous]
public class GetOffersByInquireIdEndpoint: Endpoint<GetOfferByInquireId, OfferListDto?>
{
    private readonly CoreDbContext dbContext;

    public GetOffersByInquireIdEndpoint(CoreDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public override async Task HandleAsync(GetOfferByInquireId req, CancellationToken ct)
    {
        var result = await dbContext
            .Inquiries
            .Where(iq => iq.Id == req.Id)
            .Select(iq => new OfferListDto
            {
                InquireId = iq.Id,
                MoneyInSmallestUnit = iq.MoneyInSmallestUnit,
                NumberOfInstallments = iq.NumberOfInstallments,
                Offers = dbContext
                    .Offers
                    .Where(e => e.InquireId == req.Id)
                    .Select(e => new OfferDto
                    {
                        Id = e.Id,
                        InterestRate = e.InterestRate,
                        CreationTime = e.CreationTime
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync(ct);
        
        await SendAsync(result, cancellation:ct);
    }
}