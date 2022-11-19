using Domain.Examples;
using Domain.Inquires;
using FastEndpoints;
using FastEndpoints.Swagger;
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
        builder.Services.AddDbContext<CoreDbContext>();
        builder.Services.AddScoped<ExampleService>();
        builder.Services.AddScoped<Repository<Example>>();
        builder.Services.AddScoped<Repository<Inquire>>();
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

        app.Run();
    }
}
