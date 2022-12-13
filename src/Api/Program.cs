using Domain.Examples;
using Domain.Inquiries;
using Domain.Offers;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using Services.Data;
using Services.Data.Repositories;
using Services.ValidationExtensions;
using Services.Services;

namespace Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddDbContext<CoreDbContext>(
            opts => opts.UseNpgsql(builder.Configuration["DatabaseConnectionString"])
        );
        
        builder.Services.AddScoped<ExampleService>();
        builder.Services.AddScoped<Repository<Example>>();
        builder.Services.AddScoped<Repository<Inquire>>();
        builder.Services.AddScoped<Repository<OfferTemplate>>();
        builder.Services.AddScoped<Repository<Offer>>();
        builder.Services.AddFastEndpoints();
        builder.Services.AddSwaggerDoc();
        var app = builder.Build();

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
