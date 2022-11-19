using Contracts.Inquires;
using Domain.Inquires;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Services.Data.DataMappers;
using Services.Data.Repositories;

namespace Services.Endpoints.Inquires;

[HttpPost("/inquires/createInquireAsAnonymous")]
[AllowAnonymous]
public class PostCreateInquireAsAnonymousEndpoint : Endpoint<PostCreateInquireAsAnonymous>
{
    private readonly Repository<Inquire> inquiriesRepository;

    public PostCreateInquireAsAnonymousEndpoint(Repository<Inquire> inquiriesRepository)
    {
        this.inquiriesRepository = inquiriesRepository;
    }

    public override async Task HandleAsync(PostCreateInquireAsAnonymous req, CancellationToken ct)
    {
        var inquire = new Inquire(null, req.PersonalData.ToEntity(), req.MoneyInSmallestUnit, req.NumberOfInstallments);
        await inquiriesRepository.AddAsync(inquire, ct);
        await SendAsync(new object(), cancellation: ct);
    }
}
