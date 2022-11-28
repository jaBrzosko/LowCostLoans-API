namespace Contracts.Offers;

public class OfferListDto
{
    public Guid InquireId { get; set; }
    public int MoneyInSmallestUnit { get; set; }
    public int NumberOfInstallments { get; set; }
    public List<OfferDto> Offers { get; set; }
}