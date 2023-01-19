using Services.Services.BlobStorages;
using Contracts.Api.Offers;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Services.Services.AuthServices;

namespace Services.Endpoints.Frontend.Offers;

public class GetOfferDetailsEndpoint: Endpoint<GetOfferDetails, OfferDetailsDto>
{
    private readonly IBlobStorage blobStorage;

    public GetOfferDetailsEndpoint(IBlobStorage blobStorage)
    {
        this.blobStorage = blobStorage;
    }
    
    public override void Configure()
    {
        Get("/frontend/offers/getOfferDetails");
        Roles(AuthRoles.Admin);
    }

    public override async Task HandleAsync(GetOfferDetails req, CancellationToken ct)
    {
        var result = new OfferDetailsDto()
        {
            Id = req.OfferId,
            ContractUrl = blobStorage.GetBlobReadAccessLink(blobStorage.GetContractsBlobClient(), req.OfferId.ToString() + ".contract"),
        };

        await SendAsync(result, cancellation: ct);
    }
}