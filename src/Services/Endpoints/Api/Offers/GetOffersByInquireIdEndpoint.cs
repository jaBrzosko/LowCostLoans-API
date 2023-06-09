using Contracts.Api.Offers;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Services.Data;
using Services.Middlewares;

namespace Services.Endpoints.Api.Offers;

public class GetOffersByInquireIdEndpoint : Endpoint<GetOffersByInquireId, OfferListDto?>
{
    private readonly CoreDbContext dbContext;

    public GetOffersByInquireIdEndpoint(CoreDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public override void Configure()
    {
        Get("/api/offers/getOffersByInquireId");
        AuthSchemes(ApiKeyProvider.ApiKeySchemaName);
        Summary(s =>
        {
            s.Summary = "Endpoint for getting offers created for specific inquire";
            s.Description = 
                @"""
                Endpoint for getting offers created for specific inquire.
                Offers created for inquire with given id will be returned.
                """;
        });
    }

    public override async Task HandleAsync(GetOffersByInquireId req, CancellationToken ct)
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
