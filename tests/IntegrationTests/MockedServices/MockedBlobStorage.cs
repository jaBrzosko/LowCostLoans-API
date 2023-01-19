using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Services.Services.BlobStorages;

namespace IntegrationTests.MockedServices;

public class MockedBlobStorage : IBlobStorage
{
    public BlobContainerClient GetContractsBlobClient()
    {
        return new BlobContainerClient("", "");
    }

    public Uri GetBlobReadAccessLink(BlobContainerClient containerClient, string fileNameWithExtension)
    {
        return new Uri("/valid/uri");
    }

    public Task SaveFileToBlob(Guid offerId, IFormFile file, CancellationToken ct)
    {
        return Task.CompletedTask;
    }
}
