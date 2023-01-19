namespace Contracts.Frontend.Employees;

public class PostRegister
{
    public string UserName { get; set; }
    public string Password { get; set; }

    public static class ErrorCodes
    {
        public const int UserNameIsEmpty = 1;
        public const int UserNameIsTooLong = 2;
        public const int UserNameIsAlreadyTaken = 3;
        public const int PasswordIsTooShort = 4;
        public const int PasswordIsTooLong = 5;
    }
}
