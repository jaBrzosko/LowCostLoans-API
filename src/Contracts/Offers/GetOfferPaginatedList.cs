using Contracts.Common;

namespace Contracts.Offers;

public class GetOfferPaginatedList: GetPaginatedList
{
    public bool ShowAscending { get; set; }
    public OfferSortEnum SortByElement { get; set; }
    
    public int? FilterMoneyGreaterThan { get; set; }
    public int? FilterMoneyLessThan { get; set; }
    public int? FilterInstallmentsGreaterThan { get; set; }
    public int? FilterInstallmentsLessThan { get; set; }
    public DateTime? FilterCreationTimeLaterThan { get; set; }
    public DateTime? FilterCreationTimeEarlierThan { get; set; }
    public List<OfferStatusTypeDto>? FilterOfferStatusTypes { get; set; }
}