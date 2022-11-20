using Contracts.Examples;
using Domain.Examples;
using FastEndpoints;
using Microsoft.AspNetCore.Server.HttpSys;
using Services.Data;
using Services.Data.Repositories;

namespace Services.Endpoints.Examples;

public class PostExampleEndpoint : Endpoint<PostExample>
{
    private readonly Repository<Example> examplesRepository;
    
    public PostExampleEndpoint(Repository<Example> examplesRepository)
    {
        this.examplesRepository = examplesRepository;
    }

    public override void Configure()
    {
        Post("/example");
        AllowAnonymous();
    }

    public override async Task HandleAsync(PostExample req, CancellationToken ct)
    {
        var example = new Example(req.Name);
        await examplesRepository.AddAsync(example, ct);
        await SendAsync(new object(), cancellation: ct);
    }
}
