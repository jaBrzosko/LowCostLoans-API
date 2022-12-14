using Contracts.Common;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Services.Data;
using Contracts.Offers;
using Domain.Inquiries;
using Domain.Offers;


namespace Services.Endpoints.Inquiries;
[HttpGet("/offers/getOfferList")]
[AllowAnonymous]
public class GetOfferPaginatedListEndpoint: Endpoint<GetOfferPaginatedList, PaginationResultDto<FullOfferDto>>
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

        var query = dbContext
            .Offers
            .Where(x => req.ShowCreated || x.Status != OfferStatus.Created);
        var inqs = query
            .Skip(start)
            .Take(Math.Clamp(req.PageSize, minPageSize, maxPageSize))
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
            .ToList();

        var count = query.Count();
        
        var result = new PaginationResultDto<FullOfferDto>
        {
            Results = inqs,
            Offset = start,
            TotalCount = count
        };

        await SendAsync(result, cancellation:ct);
    }
}