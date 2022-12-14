using Contracts.Common;

namespace Contracts.Offers;

public class GetOfferPaginatedList: GetPaginatedList
{
    public bool ShowCreated { get; set; }
}