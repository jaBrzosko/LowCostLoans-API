using System.Net;
using Contracts.Frontend.Employees;
using FastEndpoints;
using FluentAssertions;
using Services.Endpoints.Frontend.Employees;
using Xunit;

namespace IntegrationTests.Api.FrontLogin;
public class AccountRegisters : TestBase
{
    public AccountRegisters(ApiWebFactory apiWebFactory) : base(apiWebFactory)
    { }
    
    [Fact]
    public async Task Test()
    {
        var result = await AnonymousClient.POSTAsync<PostRegisterEndpoint, PostRegister>(new PostRegister()
        {
            UserName = "username",
            Password = "password",
        });

        result!.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}

public class AccountDoesNotRegisterBecauseOfTooShortPassword : TestBase
{
    public AccountDoesNotRegisterBecauseOfTooShortPassword(ApiWebFactory apiWebFactory) : base(apiWebFactory)
    { }
    
    [Fact]
    public async Task Test()
    {
        var result = await AnonymousClient.POSTAsync<PostRegisterEndpoint, PostRegister>(new PostRegister()
        {
            UserName = "username",
            Password = "short",
        });

        result!.StatusCode.Should().Be((HttpStatusCode)400);
    }
}

public class AccountDoesNotRegisterBecauseOfTakenUsername : TestBase
{
    public AccountDoesNotRegisterBecauseOfTakenUsername(ApiWebFactory apiWebFactory) : base(apiWebFactory)
    { }
    
    [Fact]
    public async Task Test()
    {
        var result = await AnonymousClient.POSTAsync<PostRegisterEndpoint, PostRegister>(new PostRegister()
        {
            UserName = "username",
            Password = "password",
        });

        result!.StatusCode.Should().Be(HttpStatusCode.OK);
    
        result = await AnonymousClient.POSTAsync<PostRegisterEndpoint, PostRegister>(new PostRegister()
        {
            UserName = "username",
            Password = "password",
        });

        result!.StatusCode.Should().Be((HttpStatusCode)400);
    }
}

public class LoginReturnsToken : TestBase
{
    public LoginReturnsToken(ApiWebFactory apiWebFactory) : base(apiWebFactory)
    { }
    
    [Fact]
    public async Task Test()
    {
        var registrationResult = await AnonymousClient.POSTAsync<PostRegisterEndpoint, PostRegister>(new PostRegister()
        {
            UserName = "username",
            Password = "password",
        });

        registrationResult!.StatusCode.Should().Be(HttpStatusCode.OK);

        var loginResult = await AnonymousClient.POSTAsync<PostLoginEndpoint, PostLogin, LoginResponseDto>(new PostLogin()
        {
            UserName = "username",
            Password = "password",
        });

        loginResult.response.StatusCode.Should().Be(HttpStatusCode.OK);
        loginResult.result.AreCredentialsValid.Should().BeTrue();
        loginResult.result.Token.Should().NotBeEmpty();
    }
}

public class LoginReturnsInvalidCredentials : TestBase
{
    public LoginReturnsInvalidCredentials(ApiWebFactory apiWebFactory) : base(apiWebFactory)
    { }
    
    [Fact]
    public async Task Test()
    {
        var registrationResult = await AnonymousClient.POSTAsync<PostRegisterEndpoint, PostRegister>(new PostRegister()
        {
            UserName = "username",
            Password = "password",
        });

        registrationResult!.StatusCode.Should().Be(HttpStatusCode.OK);

        var loginResult = await AnonymousClient.POSTAsync<PostLoginEndpoint, PostLogin, LoginResponseDto>(new PostLogin()
        {
            UserName = "not-username",
            Password = "password",
        });

        loginResult.response.StatusCode.Should().Be(HttpStatusCode.OK);
        loginResult.result.AreCredentialsValid.Should().BeFalse();
        loginResult.result.Token.Should().BeNullOrEmpty();
    }
}
