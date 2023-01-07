using Contracts.Common;
using Contracts.Frontend.Offers;
using Contracts.Shared.Offers;
using Domain.Offers;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Services.Data;

namespace Services.Endpoints.Frontend.Offers;

[HttpGet("/frontend/offers/getOfferList")]
[AllowAnonymous]
public class GetOfferPaginatedListEndpoint: Endpoint<GetOfferPaginatedList, PaginationResultDto<FullOfferDto>>
{
    private const int MinPageSize = 1;
    private const int MaxPageSize = 100;
    
    private readonly CoreDbContext dbContext;

    public GetOfferPaginatedListEndpoint(CoreDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public override async Task HandleAsync(GetOfferPaginatedList req, CancellationToken ct)
    {
        int pageSize = Math.Clamp(req.PageSize, MinPageSize, MaxPageSize);
        int start = req.PageNumber * pageSize;

        var query = dbContext
            .Offers
            .Where(x => req.ShowCreated || x.Status != OfferStatus.Created);
        
        var inqs = await query
            .Skip(start)
            .Take(pageSize)
            .Select(o => new FullOfferDto
            {
                Id = o.Id,
                InquireId = o.InquireId,
                CreationTime = o.CreationTime,
                InterestRate = o.InterestRate,
                MoneyInSmallestUnit = o.MoneyInSmallestUnit,
                NumberOfInstallments = o.NumberOfInstallments,
                Status = (OfferStatusTypeDto)o.Status
            })
            .ToListAsync(ct);

        var count = await query.CountAsync(ct);
        
        var result = new PaginationResultDto<FullOfferDto>
        {
            Results = inqs,
            Offset = start,
            TotalCount = count
        };

        await SendAsync(result, cancellation:ct);
    }
}
