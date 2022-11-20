namespace Contracts.Offers;

public class OfferDto
{
    public Guid Id { get; set; }
    public int InterestRate { get; set; }
    public DateTime CreationTime { get; set; }
}