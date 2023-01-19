using System.Security.Claims;
using AspNetCore.Authentication.ApiKey;

namespace Services.Middlewares;

public class ApiKeyProvider : IApiKeyProvider
{
    public const string ApiKeySchemaName = "ApiKey";
    
    public Task<IApiKey> ProvideAsync(string key)
    {
        return Task.FromResult<IApiKey>(new ApiKey { Key = key });
    }
}

public class ApiKey : IApiKey
{
    public string Key { get; set; }
    public string OwnerName { get; } = "Owner";
    public IReadOnlyCollection<Claim> Claims { get; } = new List<Claim>();
}
