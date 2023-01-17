using System.Text.Json.Serialization;
using AspNetCore.Authentication.ApiKey;
using Domain.Employees;
using Contracts.Shared.Offers;
using Contracts.Shared.Users;
using Domain.Inquiries;
using Domain.Offers;
using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using NSwag;
using Services.Configurations;
using Services.Data;
using Services.Data.Repositories;
using Services.DtoParsers;
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
        builder.Services.AddSingleton(new JwtTokenConfiguration(builder.Configuration["JWTSigningKey"]));

        builder.Services.AddTransient<IBlobStorage, BlobStorage>();
        
        builder.Services.AddScoped<Repository<Inquire>>();
        builder.Services.AddScoped<Repository<OfferTemplate>>();
        builder.Services.AddScoped<Repository<Offer>>();
        builder.Services.AddScoped<Repository<Employee>>();
        
        builder.Services.AddFastEndpoints();
        
        builder.Services.AddSwaggerDoc(s =>
        {
            s.AllowNullableBodyParameters = true;
            s.AddAuth(ApiKeyProvider.ApiKeySchemaName, new()
            {
                Name = ApiKeyMiddleware.ApiKeyHeaderName,
                In = OpenApiSecurityApiKeyLocation.Header,
                Type = OpenApiSecuritySchemeType.ApiKey,
            });
        },
        serializerSettings: x =>
        {
            x.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        });
        
        builder.Services.AddCors(options =>
            options.AddDefaultPolicy(policy =>
            {
                policy.SetIsOriginAllowed(host => host.StartsWith("http://localhost:3000")) // TODO: load ir from env
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            }));
        
        builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = ApiKeyProvider.ApiKeySchemaName; // I don't know what it is doing
                options.DefaultChallengeScheme = ApiKeyProvider.ApiKeySchemaName; // but I don't think it matters until it works
            })
            .AddApiKeyInHeader<ApiKeyProvider>(ApiKeyProvider.ApiKeySchemaName, options =>
            {
                options.Realm = "Sample Web API";
                options.KeyName = ApiKeyMiddleware.ApiKeyHeaderName;
            });

        // TODO: load it from env
        builder.Services.AddAuthenticationJWTBearer("UWaNHq1sR+3HEYyrcqO1MLa4zgtR9mYHW/wRYNsBzKRlqBMUD8U3sLUS0+j2RsN2tfNV4rQhhxfcmNmDldk94EOtDiAxg8By6YUod0fXIgWGykeb7VYg5s/NzS1UTTe8Fj7ddB522HwR3iCz97sF3H2oUW0MFYtJr9eF61MG+ZHbaw4FWeqGwqc9W0is/Q4ceLzBR3ndS+gsT/5sdMVpAt+oVa0Z08WG0BCRJrFyJhcxOkC2UGGGQVxcGUHS/ICP5zgWcOp3/iDswC6MBkl3W1T4BFmGyrBhjArGWaCwo2ae0/Z0rvSkeERgF4+AMFNRIjAYEcERFUhG1kgwL1/vAw==");

        var app = builder.Build();

        app.UseRouting();
        
        app.UseWhen(context => context.Request.Path.StartsWithSegments("/api"), appBuilder =>
        {
            appBuilder.UseMiddleware<ApiKeyMiddleware>();
        });
        
        app.UseWhen(context => context.Request.Path.StartsWithSegments("/frontend"), appBuilder =>
        {
            appBuilder.UseCors();
        });
        
        app.UseAuthentication();
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

            c.Binding.ValueParserFor<List<OfferStatusTypeDto>>(DtoListParser<OfferStatusTypeDto>.Parse);
            c.Binding.ValueParserFor<List<GovernmentIdTypeDto>>(DtoListParser<GovernmentIdTypeDto>.Parse);
            c.Binding.ValueParserFor<List<JobTypeDto>>(DtoListParser<JobTypeDto>.Parse);

            c.Serializer.Options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
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