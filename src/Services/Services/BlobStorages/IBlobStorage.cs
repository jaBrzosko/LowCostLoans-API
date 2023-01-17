using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;

namespace Services.Services.BlobStorages;

public interface IBlobStorage
{
    public BlobContainerClient GetContractsBlobClient();
    public Uri GetBlobReadAccessLink(BlobContainerClient containerClient, string fileNameWithExtension);
    public Task SaveFileToBlob(Guid offerId, IFormFile file, CancellationToken ct);
}
