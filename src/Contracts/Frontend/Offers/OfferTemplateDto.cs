namespace Contracts.Frontend.Offers;

public class OfferTemplateDto
{
    public Guid Id { get; set; }
    public int MinimumMoneyInSmallestUnit { get; set; }
    public int MaximumMoneyInSmallestUnit { get; set; }
    public int MinimumNumberOfInstallments { get; set; }
    public int MaximumNumberOfInstallments { get; set; }
    public int InterestRate { get; set; }
    public DateTime CreationTime { get; set; }
}
