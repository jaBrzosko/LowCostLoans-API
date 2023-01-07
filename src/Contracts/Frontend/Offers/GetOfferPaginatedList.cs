using Contracts.Common;

namespace Contracts.Frontend.Offers;

public class GetOfferPaginatedList: GetPaginatedList
{
    public bool ShowCreated { get; set; }
}