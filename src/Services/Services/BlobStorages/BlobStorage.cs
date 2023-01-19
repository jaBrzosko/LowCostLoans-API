using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Azure;
using Services.Configurations;

namespace Services.Services.BlobStorages;

public class BlobStorage : IBlobStorage
{
    private const string ContractsContainer = "contracts";
    
    private readonly BlobServiceClient blobClient;
    private readonly StorageSharedKeyCredential credentials;
    
    public BlobStorage(BlobStorageConfiguration configuration)
    {
        blobClient = new BlobServiceClient(configuration.ConnectionString);
        var parts = BlobStorageConnectionStringParser.Parse(configuration.ConnectionString);
        credentials = new(parts["AccountName"], parts["AccountKey"]);
    }
    
    public BlobContainerClient GetContractsBlobClient()
    {
        return blobClient.GetBlobContainerClient(ContractsContainer);
    }
    
    public Uri GetBlobReadAccessLink(BlobContainerClient containerClient, string fileNameWithExtension)
    {
        var client = containerClient.GetBlobClient(fileNameWithExtension);
    
        var escapedName = Uri.EscapeDataString(fileNameWithExtension);
        var builder = new BlobSasBuilder()
        {
            StartsOn = DateTimeOffset.UtcNow.AddMinutes(-10),
            ExpiresOn = DateTimeOffset.UtcNow.AddHours(12),
            BlobContainerName = client.BlobContainerName,
            BlobName = client.Name,
            Resource = "b",
            ContentDisposition = $"inline; filename={escapedName}; filename*=UTF-8''{escapedName}",
        };
    
        builder.SetPermissions(BlobSasPermissions.Read);
    
        return new BlobUriBuilder(client.Uri)
        {
            Sas = builder.ToSasQueryParameters(credentials),
        }.ToUri();
    }

    public async Task SaveFileToBlob(Guid offerId, IFormFile file, CancellationToken ct)
    {
        var containerClient = blobClient.GetBlobContainerClient(ContractsContainer);
        var fileClient = containerClient.GetBlobClient($"{offerId.ToString()}.contract");
        var stream = file.OpenReadStream();
        BinaryReader reader = new BinaryReader(stream);

        byte[] buffer = new byte[stream.Length];

        reader.Read(buffer, 0, buffer.Length);

        BinaryData binaryData = new BinaryData(buffer);
        await fileClient.UploadAsync(binaryData, ct);
        stream.Close();
    }
}
