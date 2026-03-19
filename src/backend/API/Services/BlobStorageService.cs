using Azure.Storage.Blobs;

namespace API.Services
{
    public class BlobStorageService : IStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _publicBaseUrl;

        public BlobStorageService(BlobServiceClient blobServiceClient, IConfiguration configuration)
        {
            _blobServiceClient = blobServiceClient;
            _publicBaseUrl = configuration["BlobStorage:PublicBaseUrl"] ?? string.Empty;
        }

        public async Task<string> UploadFileAsync(IFormFile file, string containerName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync();

            var blobName = $"{Guid.NewGuid()}_{file.FileName}";
            var blobClient = containerClient.GetBlobClient(blobName);
            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, true);
            }

            if (!string.IsNullOrWhiteSpace(_publicBaseUrl))
            {
                return $"{_publicBaseUrl.TrimEnd('/')}/{containerName}/{Uri.EscapeDataString(blobName)}";
            }

            return blobClient.Uri.ToString();
        }
    }
}
