namespace Domain.Employees;

public class Employee
{
    public const int MinPasswordLength = 6;
    public const int MaxPasswordLength = 32;
    
    public Guid Id { get; private init; }
    public string UserName { get; private set; }
    public string PasswordHash { get; private set; }

    private Employee()
    { }

    public Employee(string userName, string passwordHash)
    {
        Id = Guid.NewGuid();
        UserName = userName;
        PasswordHash = passwordHash;
    }
}
