using Contracts.Common;
using Contracts.Inquiries;
using Contracts.Shared.Users;

namespace Contracts.Frontend.Inquiries;

public class GetInquirePaginatedList : GetPaginatedList
{
    public bool ShowAscending { get; set; }
    public InquireSortEnum SortByElement { get; set; }
    public int? MoneyGreaterThanFilter { get; set; }
    public int? MoneyLessThanFilter { get; set; }
    public int? InstallmentsGreaterThanFilter { get; set; }
    public int? InstallmentsLessThanFilter { get; set; }
    public DateTime? CreationTimeLaterThanFilter { get; set; }
    public DateTime? CreationTimeEarlierThanFilter { get; set; }
    public string? NameOrSurnameFilter { get; set; }
    public string? GovernmentIdFilter { get; set; }
    public List<GovernmentIdTypeDto>? GovernmentIdTypesFilter { get; set; }
    public List<JobTypeDto>? JobTypesFilter { get; set; }
}