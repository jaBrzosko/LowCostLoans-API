using Contracts.Users;

namespace Contracts.Inquiries;

public class InquireDto
{
    public Guid Id { get; set; }
    public PersonalDataDto? PersonalData { get; set; }
    public int MoneyInSmallestUnit { get; set; }
    public int NumberOfInstallments { get; set; }
    public DateTime CreationTime { get; set; }
}