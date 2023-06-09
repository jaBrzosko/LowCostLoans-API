using Contracts.Frontend.Offers;
using Domain.Offers;
using FastEndpoints;
using Services.Data.Repositories;
using Services.Services.AuthServices;

namespace Services.Endpoints.Frontend.Offers;

public class PostCreateOfferTemplateEndpoint : Endpoint<PostCreateOfferTemplate>
{
    private readonly Repository<OfferTemplate> offerTemplatesRepository;

    public PostCreateOfferTemplateEndpoint(Repository<OfferTemplate> offerTemplatesRepository)
    {
        this.offerTemplatesRepository = offerTemplatesRepository;
    }

    public override void Configure()
    {
        Post("/frontend/offers/createOfferTemplate");
        Roles(AuthRoles.Admin);
        Summary(s =>
        {
            s.Summary = "Endpoint for creating offer template";
            s.Description = 
                @"""
                Endpoint for creating offer template.
                Created offer template will be used to create offers for inquiries.
                """;
        });
    }

    public override async Task HandleAsync(PostCreateOfferTemplate req, CancellationToken ct)
    {
        var offerTemplate = new OfferTemplate(req.MinimumMoneyInSmallestUnit, req.MaximumMoneyInSmallestUnit,
            req.MinimumNumberOfInstallments, req.MaximumNumberOfInstallments, req.InteresetRate);
        await offerTemplatesRepository.AddAsync(offerTemplate, ct);
        await SendAsync(new object(), cancellation: ct);
    }
}
