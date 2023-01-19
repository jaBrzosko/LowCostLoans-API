using Contracts.Common;
using Contracts.Frontend.Offers;
using FastEndpoints;
using Services.Data;
using Services.Endpoints.Helpers;
using Services.Services.AuthServices;

namespace Services.Endpoints.Frontend.Offers;

public class GetOffersTemplateEndpoint : Endpoint<GetOffersTemplate, PaginationResultDto<OfferTemplateDto>>
{
    private readonly CoreDbContext dbContext;

    public GetOffersTemplateEndpoint(CoreDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public override void Configure()
    {
        Get("/frontend/offers/getOfferTemplates");
        Roles(AuthRoles.Admin);
    }

    public override async Task HandleAsync(GetOffersTemplate req, CancellationToken ct)
    {
        var result = await dbContext
            .OfferTemplates
            .OrderByDescending(ot => ot.CreationTime)
            .Select(ot => new OfferTemplateDto()
            {
                Id = ot.Id,
                InterestRate = ot.InterestRate,
                MaximumNumberOfInstallments = ot.MaximumNumberOfInstallments,
                MinimumNumberOfInstallments = ot.MinimumNumberOfInstallments,
                MaximumMoneyInSmallestUnit = ot.MaximumMoneyInSmallestUnit,
                MinimumMoneyInSmallestUnit = ot.MinimumMoneyInSmallestUnit,
                CreationTime = ot.CreationTime,
            })
            .GetPaginatedResultAsync(req, ct);

        await SendAsync(result, cancellation: ct);
    }
}
