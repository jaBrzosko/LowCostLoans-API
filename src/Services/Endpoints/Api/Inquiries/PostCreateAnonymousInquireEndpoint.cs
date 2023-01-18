using Contracts.Api.Inquiries;
using Contracts.Common;
using Domain.Inquiries;
using Domain.Offers;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Services.Data;
using Services.Data.DataMappers;
using Services.Data.Repositories;
using Services.Middlewares;

namespace Services.Endpoints.Api.Inquiries;

public class PostCreateAnonymousInquireEndpoint : Endpoint<PostCreateAnonymousInquire>
{
    private readonly Repository<Inquire> inquiriesRepository;
    private readonly Repository<Offer> offersRepository;
    private readonly CoreDbContext dbContext;

    public PostCreateAnonymousInquireEndpoint(
        Repository<Inquire> inquiriesRepository,
        Repository<Offer> offersRepository,
        CoreDbContext dbContext)
    {
        this.inquiriesRepository = inquiriesRepository;
        this.offersRepository = offersRepository;
        this.dbContext = dbContext;
    }

    public override void Configure()
    {
        Post("/api/inquiries/createAnonymousInquire");
        AuthSchemes(ApiKeyProvider.ApiKeySchemaName);
        Summary(s =>
        {
            s.Summary = "Endpoint for creating anonymous inquire";
            s.Description = 
                @"""
                Endpoint for creating anonymous inquire.
                Offers for this inquire will be created from templates.
                """;
        });
    }

    public override async Task HandleAsync(PostCreateAnonymousInquire req, CancellationToken ct)
    {
        var inquire = new Inquire(req.PersonalData.ToEntity(), req.MoneyInSmallestUnit, req.NumberOfInstallments);
        inquiriesRepository.Add(inquire);

        var offers = await dbContext
            .OfferTemplates
            .Where(x =>
                x.MinimumMoneyInSmallestUnit <= req.MoneyInSmallestUnit &&
                x.MaximumMoneyInSmallestUnit >= req.MoneyInSmallestUnit &&
                x.MinimumNumberOfInstallments <= req.NumberOfInstallments &&
                x.MaximumNumberOfInstallments >= req.NumberOfInstallments)
            .Select(ot => new Offer(inquire.Id, ot.InterestRate, req.MoneyInSmallestUnit, req.NumberOfInstallments))
            .ToListAsync(ct);
        foreach (var offer in offers)
        {
            offersRepository.Add(offer);
        }

        await dbContext.SaveChangesAsync(ct);

        var response = new PostResponseWithIdDto { Id = inquire.Id };
        await SendAsync(response, cancellation: ct);
    }
}
