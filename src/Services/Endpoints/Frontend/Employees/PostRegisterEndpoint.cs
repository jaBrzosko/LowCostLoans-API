using Contracts.Frontend.Employees;
using Domain.Employees;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Services.Data.Repositories;
using Services.Services.AuthServices;

namespace Services.Endpoints.Frontend.Employees;

public class PostRegisterEndpoint : Endpoint<PostRegister>
{
    private readonly Repository<Employee> employees;

    public PostRegisterEndpoint(Repository<Employee> employees)
    {
        this.employees = employees;
    }

    public override void Configure()
    {
        Get("/frontend/register");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Endpoint for registration";
            s.Description = 
                @"""
                Endpoint for registration.
                For given credentials account is created if credentials are valid.
                """;
        });
    }

    public override async Task HandleAsync(PostRegister req, CancellationToken ct)
    {
        var passwordHash = AuthService.GetHash(req.Password);
        var employee = new Employee(req.UserName, passwordHash);

        await employees.AddAsync(employee, ct);

        await SendAsync(null, cancellation: ct);
    }
}
