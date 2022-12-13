using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;

namespace Services.Services.BlobStorages;

public class BlobStorage
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
}
