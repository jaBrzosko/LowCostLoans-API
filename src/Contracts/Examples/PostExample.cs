namespace Contracts.Examples;

public class PostExample
{
    public string Name { get; set; }

    public static class ErrorCodes
    {
        public const int NameIsEmpty = 1;
        public const int NameIsTooLong = 2;
    }
}
