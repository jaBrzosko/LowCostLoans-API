using Contracts.Common;
using Contracts.Offers;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Services.Data;

namespace Services.Endpoints.Offers;
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
            .Offers.AsQueryable();

        // Filter number of installments
        if (req.FilterInstallmentsGreaterThan != null)
            query = query.Where(x => x.NumberOfInstallments > req.FilterInstallmentsGreaterThan);
        if(req.FilterInstallmentsLessThan != null)
            query = query.Where(x => x.NumberOfInstallments < req.FilterInstallmentsLessThan);
        
        // Filter amount of money
        if (req.FilterMoneyGreaterThan != null)
            query = query.Where(x => x.MoneyInSmallestUnit > req.FilterMoneyGreaterThan);
        if (req.FilterMoneyLessThan != null)
            query = query.Where(x => x.MoneyInSmallestUnit < req.FilterMoneyLessThan);
        
        // Filter creation time
        if (req.FilterCreationTimeLaterThan != null)
            query = query.Where(x => x.CreationTime > req.FilterCreationTimeLaterThan);
        if(req.FilterCreationTimeEarlierThan != null)
            query = query.Where(x => x.CreationTime < req.FilterCreationTimeEarlierThan);

        // Filter offer statuses from list
        if (req.FilterOfferStatusTypes != null)
            query = query.Where(x => req.FilterOfferStatusTypes.Contains((OfferStatusTypeDto)x.Status));
        
        // Proper sorting
        query = req.SortByElement switch
        {
            (OfferSortEnum.Installments) => req.ShowAscending
                ? query.OrderBy(x => x.NumberOfInstallments)
                : query.OrderByDescending(x => x.NumberOfInstallments),
            (OfferSortEnum.CreationTime) => req.ShowAscending
                ? query.OrderBy(x => x.CreationTime)
                : query.OrderByDescending(x => x.CreationTime),
            (OfferSortEnum.InteresetRate) => req.ShowAscending
                ? query.OrderBy(x => x.InterestRate)
                : query.OrderByDescending(x => x.InterestRate),
            (OfferSortEnum.Money) => req.ShowAscending
                ? query.OrderBy(x => x.MoneyInSmallestUnit)
                : query.OrderByDescending(x => x.MoneyInSmallestUnit),
            _ => req.ShowAscending ? query.OrderBy(x => x.Id) : query.OrderByDescending(x => x.Id)
        };

        var inqs = await query
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