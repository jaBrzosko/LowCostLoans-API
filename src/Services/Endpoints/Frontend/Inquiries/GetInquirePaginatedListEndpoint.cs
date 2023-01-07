using Contracts.Common;
using Contracts.Frontend.Inquiries;
using Contracts.Shared.Users;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Services.Data;

namespace Services.Endpoints.Frontend.Inquiries;

[HttpGet("/frontend/inquiries/getInquireList")]
[AllowAnonymous]
public class GetInquirePaginatedListEndpoint : Endpoint<GetInquirePaginatedList, PaginationResultDto<InquireDto>>
{
    private const int MinPageSize = 1;
    private const int MaxPageSize = 100;
    
    private readonly CoreDbContext dbContext;

    public GetInquirePaginatedListEndpoint(CoreDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public override async Task HandleAsync(GetInquirePaginatedList req, CancellationToken ct)
    {
        int pageSize = Math.Clamp(req.PageSize, MinPageSize, MaxPageSize);
        int start = req.PageNumber * pageSize;
        
        var inqs = await dbContext
            .Inquiries
            .Skip(start)
            .Take(pageSize)
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
