using Contracts.Shared.Offers;

namespace Contracts.Api.Offers;

public class OfferStatusDto
{
    public Guid Id { get; set; }
    public OfferStatusTypeDto Status { get; set; }
}