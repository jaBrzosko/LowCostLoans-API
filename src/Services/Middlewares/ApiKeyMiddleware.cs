using Microsoft.AspNetCore.Http;
using Services.Configurations;

namespace Services.Middlewares;

public class ApiKeyMiddleware 
{
    public const string ApiKeyHeaderName = "ApiKey";
    
    private readonly RequestDelegate next;
    private readonly string apiKey;
    
    public ApiKeyMiddleware(RequestDelegate next, ApiKeyConfiguration configuration)
    {
        this.next = next;
        apiKey = configuration.ApiKey;
    }
    
    public async Task Invoke(HttpContext context) {
        if (!context.Request.Headers.TryGetValue(ApiKeyHeaderName, out var extractedApiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Api Key was not provided ");
            return;
        }
        
        if (!apiKey.Equals(extractedApiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Unauthorized client");
            return;
        }
        
        await next(context);
    }
}
