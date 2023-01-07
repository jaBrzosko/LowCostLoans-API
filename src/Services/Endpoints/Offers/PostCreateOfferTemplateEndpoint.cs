using Contracts.Frontend.Offers;
using Domain.Offers;
using FastEndpoints;
using Services.Data.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace Services.Endpoints.Offers;

[Obsolete]
[HttpPost("/offers/createOfferTemplate")]
[AllowAnonymous]
public class PostCreateOfferTemplateEndpoint : Endpoint<PostCreateOfferTemplate>
{
    private readonly Repository<OfferTemplate> offerTemplatesRepository;

    public PostCreateOfferTemplateEndpoint(Repository<OfferTemplate> inquiriesRepository)
    {
        this.offerTemplatesRepository = inquiriesRepository;
    }

    public override async Task HandleAsync(PostCreateOfferTemplate req, CancellationToken ct)
    {
        var offerTemplate = new OfferTemplate(req.MinimumMoneyInSmallestUnit, req.MaximumMoneyInSmallestUnit,
            req.MinimumNumberOfInstallments, req.MaximumNumberOfInstallments, req.InteresetRate);
        await offerTemplatesRepository.AddAsync(offerTemplate, ct);
        await SendAsync(new object(), cancellation: ct);
    }
}