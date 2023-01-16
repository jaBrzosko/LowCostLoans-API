using Contracts.Common;
using Contracts.Offers;
using Contracts.Shared.Offers;

namespace Contracts.Frontend.Offers;

public class GetOfferPaginatedList: GetPaginatedList
{
    public bool ShowAscending { get; set; }
    public OfferSortEnum SortByElement { get; set; }
    
    public int? MoneyGreaterThanFilter { get; set; }
    public int? MoneyLessThanFilter { get; set; }
    public int? InstallmentsGreaterThanFilter { get; set; }
    public int? InstallmentsLessThanFilter { get; set; }
    public DateTime? CreationTimeLaterThanFilter { get; set; }
    public DateTime? CreationTimeEarlierThanFilter { get; set; }
    public List<OfferStatusTypeDto>? OfferStatusTypesFilter { get; set; }
}