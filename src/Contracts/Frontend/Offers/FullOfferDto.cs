using Contracts.Frontend.Inquiries;
using Contracts.Shared.Offers;

namespace Contracts.Frontend.Offers;

public class FullOfferDto
{
    public Guid Id { get; set; }
    public InquireDto Inquire { get; set; }
    public int InterestRate { get; set; }
    public int MoneyInSmallestUnit { get; set; }
    public int NumberOfInstallments { get; set; }
    public DateTime CreationTime { get; set; }
    public OfferStatusTypeDto Status { get; set; }
}