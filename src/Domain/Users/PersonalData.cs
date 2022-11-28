namespace Domain.Users;

public record PersonalData(string FirstName, string LastName, string GovernmentId, GovernmentIdType GovernmentIdType, JobType JobType);

public enum GovernmentIdType
{
    Pesel = 0,
}

public enum JobType
{
    SomeJobType = 0,
}
