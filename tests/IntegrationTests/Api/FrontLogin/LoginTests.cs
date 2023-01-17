using System.Net;
using Contracts.Frontend.Employees;
using FastEndpoints;
using FluentAssertions;
using Services.Endpoints.Frontend.Employees;
using Xunit;

namespace IntegrationTests.Api.FrontLogin;

public class LoginTests : TestBase
{
    public LoginTests(ApiWebFactory apiWebFactory) : base(apiWebFactory)
    { }

    [Fact]
    public async Task Account_registers()
    {
        var result = await AnonymousClient.POSTAsync<PostRegisterEndpoint, PostRegister>(new PostRegister()
        {
            UserName = "username",
            Password = "password",
        });

        result!.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task Account_does_not_register_because_of_too_short_password()
    {
        var result = await AnonymousClient.POSTAsync<PostRegisterEndpoint, PostRegister>(new PostRegister()
        {
            UserName = "username",
            Password = "short",
        });

        result!.StatusCode.Should().Be((HttpStatusCode)400);
    }
    
    [Fact]
    public async Task Account_does_not_register_because_of_taken_username()
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

    [Fact]
    public async Task Login_returns_token()
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
    
    [Fact]
    public async Task Login_returns_invalid_credentials()
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
