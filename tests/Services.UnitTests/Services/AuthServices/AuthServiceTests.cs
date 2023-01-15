using FluentAssertions;
using Services.Services.AuthServices;
using Xunit;

namespace Services.UnitTests.Services.AuthServices;

public class AuthServiceTests
{
    [Fact]
    public void Passwords_match()
    {
        var password = "password";
        var hash = AuthService.GetHash(password);

        AuthService.DoesPasswordsMatch(hash, password).Should().BeTrue();
    }
    
    [Fact]
    public void Passwords_are_different()
    {
        var password1 = "password1";
        var password2 = "password2";
        var hash2 = AuthService.GetHash(password2);

        AuthService.DoesPasswordsMatch(hash2, password1).Should().BeFalse();
    }
    
    [Fact]
    public void Password_gives_two_different_hashes()
    {
        var password = "password";
        var hash1 = AuthService.GetHash(password);
        var hash2 = AuthService.GetHash(password);

        AuthService.DoesPasswordsMatch(hash1, password).Should().BeTrue();
        AuthService.DoesPasswordsMatch(hash2, password).Should().BeTrue();
        hash1.Should().NotBe(hash2);
    }
}
