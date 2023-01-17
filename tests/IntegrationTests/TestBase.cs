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
        ApiClient.DefaultRequestHeaders.Add(ApiKeyMiddleware.ApiKeyHeaderName, "api-key");
    }
}
