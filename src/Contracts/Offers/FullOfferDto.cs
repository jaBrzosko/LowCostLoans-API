namespace Contracts.Offers;

public class FullOfferDto
{
    public Guid Id { get; set; }
    public Guid InquireId { get; set; }
    public int InterestRate { get; set; }
    public int MoneyInSmallestUnit { get; set; }
    public int NumberOfInstallments { get; set; }
    public DateTime CreationTime { get; set; }
    public OfferStatusTypeDto Status { get; set; }
}