using Contracts.Common;
using Contracts.Inquiries;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Services.Data;
using Contracts.Offers;
using Contracts.Users;
using Domain.Offers;
using Microsoft.AspNetCore.Server.HttpSys;


namespace Services.Endpoints.Inquiries;
[HttpGet("/offers/getOfferList")]
[AllowAnonymous]
public class GetOfferPaginatedListEndpoint: Endpoint<GetOfferPaginatedList, PaginationResultDto<OfferDto>>
{
    private readonly CoreDbContext dbContext;
    private const int minPageSize = 1;
    private const int maxPageSize = 100;

    public GetOfferPaginatedListEndpoint(CoreDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public override async Task HandleAsync(GetOfferPaginatedList req, CancellationToken ct)
    {
        int start = req.PageNumber * Math.Clamp(req.PageSize, minPageSize, maxPageSize);

        var inqs = await dbContext
            .Offers
            .Where(x => req.ShowCreated || x.Status != OfferStatus.Created)
            .Skip(start)
            .Take(Math.Clamp(req.PageSize, minPageSize, maxPageSize))
            .Select(o => new OfferDto
            {
                Id = o.Id,
                InterestRate = o.InterestRate,
                CreationTime = o.CreationTime
            })
            .ToListAsync(ct);

        var count = await dbContext
            .Offers
            .Where(x => req.ShowCreated || x.Status != OfferStatus.Created)
            .CountAsync(ct);
        
        var result = new PaginationResultDto<OfferDto>
        {
            Results = inqs,
            Offset = start,
            TotalCount = count
        };

        await SendAsync(result, cancellation:ct);
    }
}