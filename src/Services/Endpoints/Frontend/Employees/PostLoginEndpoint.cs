using Contracts.Frontend.Employees;
using FastEndpoints;
using FastEndpoints.Security;
using Microsoft.EntityFrameworkCore;
using Services.Configurations;
using Services.Data;
using Services.Services.AuthServices;

namespace Services.Endpoints.Frontend.Employees;

public class PostLoginEndpoint : Endpoint<PostLogin, LoginResponseDto>
{
    private readonly CoreDbContext dbContext;
    private readonly string singingKey;

    public PostLoginEndpoint(CoreDbContext dbContext, JwtTokenConfiguration jwtTokenConfiguration)
    {
        this.dbContext = dbContext;
        singingKey = jwtTokenConfiguration.SigningKey;
    }

    public override void Configure()
    {
        Post("/frontend/login");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Endpoint for login";
            s.Description = 
                @"""
                Endpoint for login.
                For given credentials there is returned credentials validity and Token if credentials are valid.
                """;
        });
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
                signingKey: singingKey,
                expireAt: DateTime.UtcNow.AddDays(10),
                claims: new[] { ("UserId", employee.Id.ToString()) },
                roles: new[] { AuthRoles.Admin });
            
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
