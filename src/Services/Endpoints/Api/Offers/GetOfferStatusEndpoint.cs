using Contracts.Api.Offers;
using Contracts.Shared.Offers;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Services.Data;
using Services.Middlewares;

namespace Services.Endpoints.Api.Offers;

public class GetOfferStatusEndpoint : Endpoint<GetOfferStatus, OfferStatusDto?>
{
    private readonly CoreDbContext dbContext;

    public GetOfferStatusEndpoint(CoreDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public override void Configure()
    {
        Get("/api/offers/getOfferStatus");
        AuthSchemes(ApiKeyProvider.ApiKeySchemaName);
        Summary(s =>
        {
            s.Summary = "Endpoint for getting offer's status";
            s.Description = 
                @"""
                Endpoint for getting offer's status.
                Status of offer with given id will be returned.
                """;
        });
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
