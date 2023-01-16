using Contracts.Frontend.Employees;
using FastEndpoints;
using FastEndpoints.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Services.Data;
using Services.Services.AuthServices;

namespace Services.Endpoints.Frontend.Employees;

[HttpPost("/frontend/login")]
[AllowAnonymous]
public class PostLoginEndpoint : Endpoint<PostLogin, LoginResponseDto>
{
    private readonly CoreDbContext dbContext;

    public PostLoginEndpoint(CoreDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public override async Task HandleAsync(PostLogin req, CancellationToken ct)
    {
        var employee = await dbContext
            .Employees
            .FirstOrDefaultAsync(e => e.UserName == req.UserName, ct);

        if (employee is null)
        {
            await SendAsync(new LoginResponseDto
                {
                    Token = null, AreCredentialsValid = false,
                },
                cancellation: ct);

            return;
        }

        if (AuthService.DoesPasswordsMatch(employee.PasswordHash, req.Password))
        {
            var token = JWTBearer.CreateToken(
                signingKey: "TokenSigningKeyTokenSigningKeyTokenSigningKey",
                expireAt: DateTime.UtcNow.AddDays(1),
                claims: new[] { ("UserId", employee.Id.ToString()) },
                roles: new[] { "Admin" });
            
            await SendAsync(new LoginResponseDto
                {
                    Token = token, AreCredentialsValid = true,
                },
                cancellation: ct);
        }
        else
        {
            await SendAsync(new LoginResponseDto
                {
                    Token = null, AreCredentialsValid = false,
                },
                cancellation: ct);
        }
    }
}
