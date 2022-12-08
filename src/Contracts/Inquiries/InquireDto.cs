using Contracts.Users;

namespace Contracts.Inquiries;

public class InquireDto
{
    public Guid Id { get; set; }
    public Guid? UserId { get; private init; }
    public PersonalDataDto? PersonalData { get; private init; }
    public int MoneyInSmallestUnit { get; private init; }
    public int NumberOfInstallments { get; private init; }
    public DateTime CreationTime { get; private init; }
}