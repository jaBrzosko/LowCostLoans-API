namespace Domain.Offers;

public class Offer
{
    public Guid Id { get; private init; }
    public Guid InquireId { get; private init; }
    public int InterestRate { get; private init; }
    public int MoneyInSmallestUnit { get; private init; }
    public int NumberOfInstallments { get; private init; }
    public DateTime CreationTime { get; private init; }
    
    public Offer(Guid inquireId, int interestRate, int moneyInSmallestUnit, int numberOfInstallments)
    {
        Validate(moneyInSmallestUnit, numberOfInstallments);
        
        Id = Guid.NewGuid();
        InquireId = inquireId;
        InterestRate = interestRate;
        MoneyInSmallestUnit = moneyInSmallestUnit;
        NumberOfInstallments = numberOfInstallments;
        CreationTime = DateTime.UtcNow;
    }
    
    private Offer(){}
    
    private static void Validate(int moneyInSmallestUnit, int numberOfInstallments)
    {
        if (moneyInSmallestUnit <= 0)
        {
            throw new ArgumentException("Money has to be positive");
        }

        if (numberOfInstallments <= 0)
        {
            throw new ArgumentException("NumberOfInstallments has to be positive");
        }
    }
}