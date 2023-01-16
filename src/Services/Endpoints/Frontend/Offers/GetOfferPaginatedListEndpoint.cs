using Contracts.Common;
using Contracts.Frontend.Offers;
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
        var result = await dbContext
            .Offers
            .Where(x => req.ShowCreated || x.Status != OfferStatus.Created)
            .Select(o => new FullOfferDto
            {
                Id = o.Id,
                InquireId = o.InquireId,
                CreationTime = o.CreationTime,
                InterestRate = o.InterestRate,
                MoneyInSmallestUnit = o.MoneyInSmallestUnit,
                NumberOfInstallments = o.NumberOfInstallments,
                Status = (OfferStatusTypeDto)o.Status,
            })
            .GetPaginatedResultAsync(req, ct);
        
        await SendAsync(result, cancellation:ct);
    }
}
