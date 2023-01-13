using Contracts.Shared.Users;

namespace Contracts.Api.Inquiries;

public class PostCreateAnonymousInquire
{
    public int MoneyInSmallestUnit { get; set; }
    public int NumberOfInstallments { get; set; }
    public PersonalDataDto PersonalData { get; set; }

    public static class ErrorCodes
    {
        public const int MoneyHasToBePositive = 1;
        public const int NumberOfInstallmentsHasToBePositive = 2;

        public class PersonalDataErrors : PersonalDataDto.ErrorCodes { }
    }
}
