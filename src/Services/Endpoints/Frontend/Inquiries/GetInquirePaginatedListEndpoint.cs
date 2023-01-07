using Contracts.Common;
using Contracts.Frontend.Inquiries;
using Contracts.Shared.Users;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Services.Data;
using Services.Endpoints.Helpers;

namespace Services.Endpoints.Frontend.Inquiries;

[HttpGet("/frontend/inquiries/getInquireList")]
[AllowAnonymous]
public class GetInquirePaginatedListEndpoint : Endpoint<GetInquirePaginatedList, PaginationResultDto<InquireDto>>
{
    private readonly CoreDbContext dbContext;

    public GetInquirePaginatedListEndpoint(CoreDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public override async Task HandleAsync(GetInquirePaginatedList req, CancellationToken ct)
    {
        var result = await dbContext
            .Inquiries
            .OrderBy(iq => iq.CreationTime)
            .Select(iq => new InquireDto
            {
                Id = iq.Id,
                PersonalData = iq.PersonalData == null
                    ? null
                    : new PersonalDataDto
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
            .GetPaginatedResultAsync(req, ct);

        await SendAsync(result, cancellation:ct);
    }
}
