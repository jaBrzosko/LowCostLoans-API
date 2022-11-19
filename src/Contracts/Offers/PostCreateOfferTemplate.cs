namespace Contracts.Offers;

public class PostCreateOfferTemplate
{
    public int MinimumMoneyInSmallestUnit { get; set; }
    public int MaximumMoneyInSmallestUnit { get; set; }
    public int MinimumNumberOfInstallments { get; set; }
    public int MaximumNumberOfInstallments { get; set; }
    public int InteresetRate { get; set; }
    
    public static class ErrorCodes
    {
        public const int MoneyHasToBePositive = 1;
        public const int NumberOfInstallmentsHasToBePositive = 2;
        public const int InterestRateHasToBeNonNegative = 3;
    }
}