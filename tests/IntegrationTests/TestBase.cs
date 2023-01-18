using Contracts.Frontend.Employees;
using FastEndpoints;
using Microsoft.Extensions.DependencyInjection;
using Services.Configurations;
using Services.Endpoints.Frontend.Employees;
using Services.Middlewares;
using Xunit;

namespace IntegrationTests;

public class TestBase : IClassFixture<ApiWebFactory>
{
    private readonly ApiWebFactory apiWebFactory;
    protected readonly HttpClient ApiClient;
    protected readonly HttpClient AnonymousClient;

    public TestBase(ApiWebFactory apiWebFactory)
    {
        this.apiWebFactory = apiWebFactory;
        AnonymousClient = apiWebFactory.CreateClient();
        ApiClient = apiWebFactory.CreateClient();
        var apiKeyConfiguration = apiWebFactory.Services.GetService<ApiKeyConfiguration>();
        ApiClient.DefaultRequestHeaders.Add(ApiKeyMiddleware.ApiKeyHeaderName, apiKeyConfiguration.ApiKey);
    }
}

public class TestBaseWithFrontendAuthentication : TestBase
{
    protected readonly HttpClient FrontendClient;
    
    public TestBaseWithFrontendAuthentication(ApiWebFactory apiWebFactory) : base(apiWebFactory)
    {
        FrontendClient = apiWebFactory.CreateClient();
        
        AnonymousClient.POSTAsync<PostRegisterEndpoint, PostRegister>(new PostRegister()
        {
            UserName = "frontend-username",
            Password = "frontend-password",
        }).Wait();
        
        var loginTask = AnonymousClient.POSTAsync<PostLoginEndpoint, PostLogin, LoginResponseDto>(new PostLogin()
        {
            UserName = "frontend-username",
            Password = "frontend-password",
        });

        loginTask.Wait();

        var token = loginTask.Result.result.Token;
        
        FrontendClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
    }
}
