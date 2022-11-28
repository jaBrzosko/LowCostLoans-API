using Contracts.Examples;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Services.Data;

namespace Services.Endpoints.Examples;

public class GetExampleEndpoint : Endpoint<GetExample, ExampleDto?>
{
    private readonly CoreDbContext dbContext;

    public GetExampleEndpoint(CoreDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public override void Configure()
    {
        Get("/example");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetExample req, CancellationToken ct)
    {
        var result = await dbContext
            .Examples
            .Where(e => e.Id == req.Id)
            .Select(e => new ExampleDto
            {
                Id = e.Id,
                Name = e.Name,
                CreationTime = e.CreationTime,
            })
            .FirstOrDefaultAsync(ct);
        
        await SendAsync(result, cancellation: ct);
    }
}
