using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Services.Middlewares;

public class ApiKeyMiddleware 
{
    private const string ApiKeyHeaderName = "ApiKey";
    
    private readonly RequestDelegate next;
    
    public ApiKeyMiddleware(RequestDelegate next)
    {
        this.next = next;
    }
    
    public async Task Invoke(HttpContext context) {
        if (!context.Request.Headers.TryGetValue(ApiKeyHeaderName, out var extractedApiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Api Key was not provided ");
            return;
        }
        
        var appSettings = context.RequestServices.GetRequiredService<IConfiguration>();
        var apiKey = appSettings.GetValue<string>(ApiKeyHeaderName);
        if (!apiKey.Equals(extractedApiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Unauthorized client");
            return;
        }
        
        await next(context);
    }
}
