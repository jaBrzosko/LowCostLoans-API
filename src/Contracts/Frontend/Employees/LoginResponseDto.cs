namespace Contracts.Frontend.Employees;

public class LoginResponseDto
{
    public string? Token { get; set; }
    public bool AreCredentialsValid { get; set; }
}
