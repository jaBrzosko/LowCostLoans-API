using Contracts.Api.Offers;
using FastEndpoints;
using Services.Middlewares;
using Services.Services.BlobStorages;

namespace Services.Endpoints.Api.Offers;

public class GetOfferContractEndpoint : Endpoint<GetOfferContract, ContractDto>
{
    private readonly IBlobStorage blobStorage;

    public GetOfferContractEndpoint(IBlobStorage blobStorage)
    {
        this.blobStorage = blobStorage;
    }

    public override void Configure()
    {
        Get("/api/offers/getOfferContract");
        AuthSchemes(ApiKeyProvider.ApiKeySchemaName);
        Summary(s =>
        {
            s.Summary = "Endpoint for getting offer's contract";
            s.Description = 
                @"""
                Endpoint for getting offer's contract.
                Template contract will be returned.
                """;
        });
    }

    public override async Task HandleAsync(GetOfferContract req, CancellationToken ct)
    {
        var result = new ContractDto()
        {
            ContractUrl = blobStorage.GetBlobReadAccessLink(blobStorage.GetContractsBlobClient(), "contract.pdf"),
        };

        await SendAsync(result, cancellation: ct);
    }
}
