using Contracts.Api.Offers;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Services.Services.BlobStorages;

namespace Services.Endpoints.Offers;

[Obsolete]
[HttpGet("/offers/getOfferContract")]
[AllowAnonymous]
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
