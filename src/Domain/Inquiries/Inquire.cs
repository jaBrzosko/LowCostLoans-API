using Domain.Users;

namespace Domain.Inquiries;

public class Inquire
{
    public Guid Id { get; private init; }
    public PersonalData PersonalData { get; private init; }
    public int MoneyInSmallestUnit { get; private init; }
    public int NumberOfInstallments { get; private init; }
    public DateTime CreationTime { get; private init; }

    public Inquire(PersonalData personalData, int moneyInSmallestUnit, int numberOfInstallments)
    {
        Validate(moneyInSmallestUnit, numberOfInstallments);
        
        Id = Guid.NewGuid();
        PersonalData = personalData;
        MoneyInSmallestUnit = moneyInSmallestUnit;
        NumberOfInstallments = numberOfInstallments;
        CreationTime = DateTime.UtcNow;
    }
    
    private Inquire() 
    { }

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
