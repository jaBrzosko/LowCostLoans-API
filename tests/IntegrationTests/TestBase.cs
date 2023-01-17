using Services.Middlewares;
using Xunit;

namespace IntegrationTests;

public class TestBase : IClassFixture<ApiWebFactory>
{
    private readonly ApiWebFactory apiWebFactory;
    protected readonly HttpClient Client;

    public TestBase(ApiWebFactory apiWebFactory)
    {
        this.apiWebFactory = apiWebFactory;
        Client = apiWebFactory.CreateClient();
    }
}

public class TestBaseWithApiKey : TestBase
{
    public TestBaseWithApiKey(ApiWebFactory apiWebFactory) : base(apiWebFactory)
    {
        Client.DefaultRequestHeaders.Add(ApiKeyMiddleware.ApiKeyHeaderName, "api-key");
    }
}
