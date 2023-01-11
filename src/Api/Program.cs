using System.Security.Claims;
using AspNetCore.Authentication.ApiKey;
using Domain.Examples;
using Domain.Inquiries;
using Domain.Offers;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using NSwag;
using Services.Configurations;
using Services.Data;
using Services.Data.Repositories;
using Services.Middlewares;
using Services.Services.BlobStorages;
using Services.ValidationExtensions;

namespace Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddDbContext<CoreDbContext>(
            opts => opts.UseNpgsql(builder.Configuration["DatabaseConnectionString"])
        );
        
        builder.Services.AddSingleton(new BlobStorageConfiguration(builder.Configuration["BlobStorageConnectionString"]));
        builder.Services.AddSingleton(new ApiKeyConfiguration(builder.Configuration["ApiKey"]));

        builder.Services.AddTransient<BlobStorage>();
        
        builder.Services.AddScoped<Repository<Example>>();
        builder.Services.AddScoped<Repository<Inquire>>();
        builder.Services.AddScoped<Repository<OfferTemplate>>();
        builder.Services.AddScoped<Repository<Offer>>();
        
        builder.Services.AddFastEndpoints();
        
        builder.Services.AddSwaggerDoc(s =>
        {
            s.AddAuth("ApiKey", new()
            {
                Name = "ApiKey",
                In = OpenApiSecurityApiKeyLocation.Header,
                Type = OpenApiSecuritySchemeType.ApiKey,
            });
        });

        builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "ApiKey"; // I don't know what it is doing
                options.DefaultChallengeScheme = "ApiKey"; // but I don't think it matters until it works
            })
            .AddApiKeyInHeader<ApiKeyProvider>("ApiKey", options =>
            {
                options.Realm = "Sample Web API";
                options.KeyName = "ApiKey";
            });

        var app = builder.Build();

        app.UseRouting();
        
        app.UseWhen(context => context.Request.Path.StartsWithSegments("/api"), appBuilder =>
        {
            appBuilder.UseMiddleware<ApiKeyMiddleware>();
            app.UseAuthentication();
        });
        
        app.UseAuthorization();
        app.UseFastEndpoints(c =>
        {
            c.Errors.ResponseBuilder = (failures, ctx, statusCode) =>
            {
                return new ValidationErrors
                {
                    StatusCode = statusCode,
                    Errors = failures
                        .Select(f => new Error
                        {
                            ErrorCode = int.Parse(f.ErrorCode),
                            ErrorMessage = f.ErrorMessage,
                        })
                        .ToList(),
                };
            };
        });
        app.UseOpenApi();
        app.UseSwaggerUi3(s => s.ConfigureDefaults());

        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            var context = services.GetRequiredService<CoreDbContext>();
            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
        }

        app.Run();
    }
}
