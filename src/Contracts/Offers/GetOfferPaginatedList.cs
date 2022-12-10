using Contracts.Common;

namespace Contracts.Offers;

public class GetOfferPaginatedList: GetPagination
{
    public bool ShowCreated { get; set; }
}