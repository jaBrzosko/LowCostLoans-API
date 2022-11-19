using Domain.Offers;
using FastEndpoints;
using Contracts.Offers;
using Services.Data.Repositories;

namespace Services.Endpoints.Offers;

[HttpPost("/offers/createOfferTemplate")]
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