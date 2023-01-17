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
    public async Task Account_registered()
    {
        var result = await AnonymousClient.POSTAsync<PostRegisterEndpoint, PostRegister>(new PostRegister()
        {
            UserName = "usename",
            Password = "password",
        });

        result!.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
