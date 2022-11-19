using Contracts.Examples;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Services.Data;

namespace Services.Endpoints.Examples;

public class GetAllExamplesEndpoint : Endpoint<GetAllExamples, List<ExampleDto>>
{
    private readonly CoreDbContext dbContext;

    public GetAllExamplesEndpoint(CoreDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public override void Configure()
    {
        Get("/allExamples");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetAllExamples req, CancellationToken ct)
    {
        var result = await dbContext
            .Examples
            .Select(e => new ExampleDto
            {
                Id = e.Id,
                Name = e.Name,
                CreationTime = e.CreationTime,
            })
            .ToListAsync(ct);
        
        await SendAsync(result, cancellation: ct);
    }
}
