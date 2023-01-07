using Contracts.Frontend.Offers;
using Domain.Offers;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Services.Data.Repositories;

namespace Services.Endpoints.Frontend.Offers;

[HttpPost("/frontend/offers/createOfferTemplate")]
[AllowAnonymous]
public class PostCreateOfferTemplateEndpoint : Endpoint<PostCreateOfferTemplate>
{
    private readonly Repository<OfferTemplate> offerTemplatesRepository;

    public PostCreateOfferTemplateEndpoint(Repository<OfferTemplate> offerTemplatesRepository)
    {
        this.offerTemplatesRepository = offerTemplatesRepository;
    }

    public override async Task HandleAsync(PostCreateOfferTemplate req, CancellationToken ct)
    {
        var offerTemplate = new OfferTemplate(req.MinimumMoneyInSmallestUnit, req.MaximumMoneyInSmallestUnit,
            req.MinimumNumberOfInstallments, req.MaximumNumberOfInstallments, req.InteresetRate);
        await offerTemplatesRepository.AddAsync(offerTemplate, ct);
        await SendAsync(new object(), cancellation: ct);
    }
}
