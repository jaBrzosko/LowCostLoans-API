using Contracts.Common;
using Contracts.Frontend.Inquiries;
using Contracts.Shared.Users;
using Domain.Inquiries;
using FastEndpoints;
using Services.Data;
using Services.Endpoints.Helpers;
using Services.Services.AuthServices;

namespace Services.Endpoints.Frontend.Inquiries;

public class GetInquirePaginatedListEndpoint : Endpoint<GetInquirePaginatedList, PaginationResultDto<InquireDto>>
{
    private readonly CoreDbContext dbContext;

    public GetInquirePaginatedListEndpoint(CoreDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public override void Configure()
    {
        Get("/frontend/inquiries/getInquireList");
        Roles(AuthRoles.Admin);
    }

    public override async Task HandleAsync(GetInquirePaginatedList req, CancellationToken ct)
    {
        var query = dbContext
            .Inquiries.AsQueryable();

        query = FilterInquireQuery(query, req);

        query = SortInquireQuery(query, req);
        
        var result = await query
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
    
    private IQueryable<Inquire> FilterInquireQuery(IQueryable<Inquire> query, GetInquirePaginatedList req)
    {
        if (req.InstallmentsGreaterThanFilter != null)
            query = query.Where(x => x.NumberOfInstallments > req.InstallmentsGreaterThanFilter);
        if(req.InstallmentsLessThanFilter != null)
            query = query.Where(x => x.NumberOfInstallments < req.InstallmentsLessThanFilter);
        
        if (req.MoneyGreaterThanFilter != null)
            query = query.Where(x => x.MoneyInSmallestUnit > req.MoneyGreaterThanFilter);
        if (req.MoneyLessThanFilter != null)
            query = query.Where(x => x.MoneyInSmallestUnit < req.MoneyLessThanFilter);
        
        if (req.CreationTimeLaterThanFilter != null)
            query = query.Where(x => x.CreationTime > req.CreationTimeLaterThanFilter);
        if(req.CreationTimeEarlierThanFilter != null)
            query = query.Where(x => x.CreationTime < req.CreationTimeEarlierThanFilter);

        if (req.NameOrSurnameFilter != null)
        {
            var filter = req.NameOrSurnameFilter.ToLower();
            query = query.Where(x => 
                x.PersonalData.FirstName.ToLower().Contains(filter) 
                || x.PersonalData.LastName.ToLower().Contains(filter));
        }

        if (req.GovernmentIdFilter != null)
        {
            var filter = req.GovernmentIdFilter.ToLower();   
            query = query.Where(x => x.PersonalData.GovernmentId.ToLower().Contains(filter));
        }

        if (req.GovernmentIdTypesFilter != null)
            query = query.Where(x => req.GovernmentIdTypesFilter.Contains((GovernmentIdTypeDto)x.PersonalData.GovernmentIdType));
        
        if (req.JobTypesFilter != null)
            query = query.Where(x => req.JobTypesFilter.Contains((JobTypeDto)x.PersonalData.JobType));
        
        return query;
    }
    
    private IQueryable<Inquire> SortInquireQuery(IQueryable<Inquire> query, GetInquirePaginatedList req)
    {
        query = req.SortByElement switch
        {
            (InquireSortEnum.Installments) => req.ShowAscending
                ? query.OrderBy(x => x.NumberOfInstallments)
                : query.OrderByDescending(x => x.NumberOfInstallments),
            (InquireSortEnum.CreationTime) => req.ShowAscending
                ? query.OrderBy(x => x.CreationTime)
                : query.OrderByDescending(x => x.CreationTime),
            (InquireSortEnum.Money) => req.ShowAscending
                ? query.OrderBy(x => x.MoneyInSmallestUnit)
                : query.OrderByDescending(x => x.MoneyInSmallestUnit),
            (InquireSortEnum.Name) => req.ShowAscending
                ? query.OrderBy(x => x.PersonalData.FirstName)
                : query.OrderByDescending(x => x.PersonalData.FirstName),
            (InquireSortEnum.Surname) => req.ShowAscending
                ? query.OrderBy(x => x.PersonalData.LastName)
                : query.OrderByDescending(x => x.PersonalData.LastName),
            (InquireSortEnum.GovernmentId) => req.ShowAscending
                ? query.OrderBy(x => x.PersonalData.GovernmentId)
                : query.OrderByDescending(x => x.PersonalData.GovernmentId),
            (InquireSortEnum.GovernmentIdType) => req.ShowAscending
                ? query.OrderBy(x => x.PersonalData.GovernmentIdType)
                : query.OrderByDescending(x => x.PersonalData.GovernmentIdType),
            (InquireSortEnum.JobType) => req.ShowAscending
                ? query.OrderBy(x => x.PersonalData.JobType)
                : query.OrderByDescending(x => x.PersonalData.JobType),
            _ => req.ShowAscending ? query.OrderBy(x => x.Id) : query.OrderByDescending(x => x.Id)
        };
        return query;
    }
}
