using Contracts.Inquiries;
using Domain.Inquiries;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Services.Data.DataMappers;
using Services.Data.Repositories;
using Contracts.Common;
using Domain.Offers;
using Microsoft.EntityFrameworkCore;
using Services.Data;

namespace Services.Endpoints.Inquiries;

[HttpPost("/inquiries/createInquireAsAnonymous")]
[AllowAnonymous]
public class PostCreateInquireAsAnonymousEndpoint : Endpoint<PostCreateInquireAsAnonymous>
{
    private readonly Repository<Inquire> inquiriesRepository;
    private readonly Repository<Offer> offersRepository;
    private readonly CoreDbContext dbContext;

    public PostCreateInquireAsAnonymousEndpoint(Repository<Inquire> inquiriesRepository, Repository<Offer> offersRepository, 
        CoreDbContext dbContext)
    {
        this.inquiriesRepository = inquiriesRepository;
        this.offersRepository = offersRepository;
        this.dbContext = dbContext;
    }

    public override async Task HandleAsync(PostCreateInquireAsAnonymous req, CancellationToken ct)
    {
        var inquire = new Inquire(null, req.PersonalData.ToEntity(), req.MoneyInSmallestUnit, req.NumberOfInstallments);
        await inquiriesRepository.AddAsync(inquire, ct);

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

        await offersRepository.SaveContext(ct);
        
        var response = new PostResponseWithIdDto { Id = inquire.Id };
        await SendAsync(response, cancellation: ct);
    }
}
