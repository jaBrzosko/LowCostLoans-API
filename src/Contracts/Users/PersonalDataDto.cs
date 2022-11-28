namespace Contracts.Users;

public class PersonalDataDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string GovernmentId { get; set; }
    public GovernmentIdTypeDto GovernmentIdType { get; set; }
    public JobTypeDto JobType { get; set; }

    public class ErrorCodes
    {
        public const int FirstNameIsEmpty = 1_001;
        public const int FirstNameIsTooLong = 1_002;
        public const int LastNameIsEmpty = 1_003;
        public const int LastNameIsTooLong = 1_004;
        public const int GovernmentIdIsEmpty = 1_005;
        public const int GovernmentIdIsTooLong = 1_006;
        public const int GovernmentIdIsInvalid = 1_007;
    }
}
