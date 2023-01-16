using Contracts.Common;
using Contracts.Frontend.Offers;
using Contracts.Offers;
using Contracts.Shared.Offers;
using Domain.Offers;
using FastEndpoints;
using Services.Data;
using Services.Endpoints.Helpers;
using Services.Services.AuthServices;

namespace Services.Endpoints.Frontend.Offers;

public class GetOfferPaginatedListEndpoint: Endpoint<GetOfferPaginatedList, PaginationResultDto<FullOfferDto>>
{
    private readonly CoreDbContext dbContext;
    private const int minPageSize = 1;
    private const int maxPageSize = 100;

    public GetOfferPaginatedListEndpoint(CoreDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public override void Configure()
    {
        Get("/frontend/offers/getOfferList");
        Roles(AuthRoles.Admin);
    }

    public override async Task HandleAsync(GetOfferPaginatedList req, CancellationToken ct)
    {
        int start = req.PageNumber * Math.Clamp(req.PageSize, minPageSize, maxPageSize);

        var query = dbContext
            .Offers.AsQueryable();

        query = FilterOfferQuery(query, req);

        query = SortOfferQuery(query, req);

        var finalQuery = query
            .Select(o => new FullOfferDto
            {
                Id = o.Id,
                InquireId = o.InquireId,
                CreationTime = o.CreationTime,
                InterestRate = o.InterestRate,
                MoneyInSmallestUnit = o.MoneyInSmallestUnit,
                NumberOfInstallments = o.NumberOfInstallments,
                Status = (OfferStatusTypeDto)o.Status
            });

        var result = await finalQuery.GetPaginatedResultAsync(req, ct);

        await SendAsync(result, cancellation:ct);
    }

    private IQueryable<Offer> FilterOfferQuery(IQueryable<Offer> query, GetOfferPaginatedList req)
    {
        if (req.InstallmentsGreaterThanFilter != null)
            query = query.Where(x => x.NumberOfInstallments > req.InstallmentsGreaterThanFilter);
        if(req.InstallmentsLessThanFilter != null)
            query = query.Where(x => x.NumberOfInstallments < req.InstallmentsLessThanFilter);
        
        if (req.MoneyGreaterThanFilter != null)
            query = query.Where(x => x.MoneyInSmallestUnit > req.MoneyGreaterThanFilter);
        if (req.MoneyLessThanFilter != null)
            query = query.Where(x => x.MoneyInSmallestUnit < req.MoneyLessThanFilter);
        
        if (req.CreationTimeLaterThanFilter != null)
            query = query.Where(x => x.CreationTime > req.CreationTimeLaterThanFilter);
        if(req.CreationTimeEarlierThanFilter != null)
            query = query.Where(x => x.CreationTime < req.CreationTimeEarlierThanFilter);

        if (req.OfferStatusTypesFilter != null)
            query = query.Where(x => req.OfferStatusTypesFilter.Contains((OfferStatusTypeDto)x.Status));
        
        return query;
    }
    
    private IQueryable<Offer> SortOfferQuery(IQueryable<Offer> query, GetOfferPaginatedList req)
    {
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
        return query;
    }
}
