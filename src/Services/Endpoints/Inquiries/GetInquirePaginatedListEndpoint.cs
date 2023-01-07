using Contracts.Common;
using Contracts.Frontend.Inquiries;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Services.Data;
using Contracts.Shared.Users;
using Microsoft.AspNetCore.Server.HttpSys;


namespace Services.Endpoints.Inquiries;
[HttpGet("/inquiries/getInquireList")]
[AllowAnonymous]
public class GetInquirePaginatedListEndpoint: Endpoint<GetInquirePaginatedList, PaginationResultDto<InquireDto>>
{
    private readonly CoreDbContext dbContext;
    private const int minPageSize = 1;
    private const int maxPageSize = 100;

    public GetInquirePaginatedListEndpoint(CoreDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public override async Task HandleAsync(GetInquirePaginatedList req, CancellationToken ct)
    {
        int start = req.PageNumber * Math.Clamp(req.PageSize, minPageSize, maxPageSize);
        var inqs = await dbContext
            .Inquiries
            .Skip(start)
            .Take(Math.Clamp(req.PageSize, minPageSize, maxPageSize))
            .Select(iq => new InquireDto
            {
                Id = iq.Id,
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
        var result = new PaginationResultDto<InquireDto>
        {
            Results = inqs,
            Offset = start,
            TotalCount = count
        };

        await SendAsync(result, cancellation:ct);
    }
}