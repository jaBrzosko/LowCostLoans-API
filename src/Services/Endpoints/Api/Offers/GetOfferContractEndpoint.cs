using Contracts.Api.Offers;
using FastEndpoints;
using Services.Services.BlobStorages;

namespace Services.Endpoints.Api.Offers;

[HttpGet("/api/offers/getOfferContract")]
public class GetOfferContractEndpoint : Endpoint<GetOfferContract, ContractDto>
{
    private readonly IBlobStorage blobStorage;

    public GetOfferContractEndpoint(IBlobStorage blobStorage)
    {
        this.blobStorage = blobStorage;
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
