using Domain.Users;

namespace Domain.Inquires;

public class Inquire
{
    public Guid Id { get; private init; }
    public Guid? UserId { get; private init; }
    public PersonalData? PersonalData { get; private init; }
    public int MoneyInSmallestUnit { get; private init; }
    public int NumberOfInstallments { get; private init; }
    public DateTime CreationTime { get; private init; }

    public Inquire(Guid? userId, PersonalData? personalData, int moneyInSmallestUnit, int numberOfInstallments)
    {
        Validate(userId, personalData, moneyInSmallestUnit, numberOfInstallments);
        
        Id = Guid.NewGuid();
        UserId = userId;
        PersonalData = personalData;
        MoneyInSmallestUnit = moneyInSmallestUnit;
        NumberOfInstallments = numberOfInstallments;
        CreationTime = DateTime.UtcNow;
    }
    
    private Inquire() 
    { }

    private static void Validate(Guid? userId, PersonalData? personalData, int moneyInSmallestUnit, int numberOfInstallments)
    {
        if (!(userId is null ^ personalData is null))
        {
            throw new ArgumentException("Exactly one of userId and personalData has to be null");
        }

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
