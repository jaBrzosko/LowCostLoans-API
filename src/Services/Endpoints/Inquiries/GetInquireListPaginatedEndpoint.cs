using Contracts.Common;
using Contracts.Inquiries;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Services.Data;
using Contracts.Offers;
using Contracts.Users;
using Microsoft.AspNetCore.Server.HttpSys;


namespace Services.Endpoints.Inquiries;
[HttpGet("/inquiries/getInquireList")]
[AllowAnonymous]
public class GetInquireListPaginatedEndpoint: Endpoint<GetInquireListPaginated, PaginationDto<InquireDto>?>
{
    private readonly CoreDbContext dbContext;

    public GetInquireListPaginatedEndpoint(CoreDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public override async Task HandleAsync(GetInquireListPaginated req, CancellationToken ct)
    {
        int start = req.PageNumber * req.PageSize;
        var inqs = await dbContext
            .Inquiries
            .Skip(start)
            .Take(req.PageSize)
            .Select(iq => new InquireDto
            {
            Id = iq.Id,
            UserId = iq.UserId,
            PersonalData = iq.PersonalData == null ? null : new PersonalDataDto
            {
                FirstName = iq.PersonalData.FirstName,
                LastName = iq.PersonalData.LastName,
                GovernmentId = iq.PersonalData.GovernmentId,
                GovernmentIdType = (GovernmentIdTypeDto)iq.PersonalData.GovernmentIdType,
                JobType = (JobTypeDto)iq.PersonalData.JobType
            },
            MoneyInSmallestUnit = iq.MoneyInSmallestUnit,
            NumberOfInstallments = iq.NumberOfInstallments,
            CreationTime = iq.CreationTime
            })
            .ToListAsync(ct);
        var count = await dbContext
            .Inquiries
            .CountAsync(ct);
        var result = new PaginationDto<InquireDto>
        {
            Results = inqs,
            Offset = start,
            TotalCount = count
        };

        await SendAsync(result, cancellation:ct);
    }
}