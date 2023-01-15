namespace Domain.Employees;

public class Employee
{
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
