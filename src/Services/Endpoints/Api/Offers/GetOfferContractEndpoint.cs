using Contracts.Api.Offers;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Services.Middlewares;
using Services.Services.BlobStorages;

namespace Services.Endpoints.Api.Offers;

public class GetOfferContractEndpoint : Endpoint<GetOfferContract, ContractDto>
{
    private readonly BlobStorage blobStorage;

    public GetOfferContractEndpoint(BlobStorage blobStorage)
    {
        this.blobStorage = blobStorage;
    }

    public override void Configure()
    {
        Get("/api/offers/getOfferContract");
        AuthSchemes(ApiKeyProvider.ApiKeySchemaName);
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
