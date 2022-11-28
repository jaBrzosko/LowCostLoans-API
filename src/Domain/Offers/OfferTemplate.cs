namespace Domain.Offers;

public class OfferTemplate
{
    public Guid Id { get; private init; }
    public int MinimumMoneyInSmallestUnit { get; private init; }
    public int MaximumMoneyInSmallestUnit { get; private init; }
    public int MinimumNumberOfInstallments { get; private init; }
    public int MaximumNumberOfInstallments { get; private init; }
    public int InterestRate { get; private init; }

    public DateTime CreationTime { get; private init; }

    public OfferTemplate(int minimumMoneyInSmallestUnit, int maximumMoneyInSmallestUnit,
        int minimumNumberOfInstallments, int maximumNumberOfInstallments, int interestRate)
    {
        Validate(minimumMoneyInSmallestUnit, maximumMoneyInSmallestUnit,
            minimumNumberOfInstallments, maximumNumberOfInstallments, interestRate);

        Id = Guid.NewGuid();
        MinimumMoneyInSmallestUnit = minimumMoneyInSmallestUnit;
        MaximumMoneyInSmallestUnit = maximumMoneyInSmallestUnit;
        MinimumNumberOfInstallments = minimumNumberOfInstallments;
        MaximumNumberOfInstallments = maximumNumberOfInstallments;
        InterestRate = interestRate;
        CreationTime = DateTime.UtcNow;
    }

    private OfferTemplate()
    { }

    private static void Validate(int minimumMoneyInSmallestUnit, int maximumMoneyInSmallestUnit,
        int minimumNumberOfInstallments, int maximumNumberOfInstallments, int interestRate)
    {
        if (minimumMoneyInSmallestUnit <= 0)
        {
            throw new ArgumentException("Money has to be positive");
        }
        if(maximumMoneyInSmallestUnit < minimumMoneyInSmallestUnit)
        {
            throw new ArgumentException("MaximumMoneyInSmallestUnit cannot be lower than MinimumMoneyInSmallestUnit");
        }
        
        if (minimumNumberOfInstallments <= 0)
        {
            throw new ArgumentException("NumberOfInstallments has to be positive");
        }
        if(maximumNumberOfInstallments < minimumNumberOfInstallments)
        {
            throw new ArgumentException("MaximumNumberOfInstallments cannot be lower than MinimumNumberOfInstallments");
        }

        if (interestRate < 0)
        {
            throw new ArgumentException("InterestRate has to be non negative");
        }
    }
}