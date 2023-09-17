using Azure.Storage.Blobs;
using EmailWebApi.Interfaces;

namespace EmailWebApi.Services
{
    public class AzureBlobService : IAzureBlobService
    {
        private readonly BlobContainerClient _blobContainer;

        public AzureBlobService()
        {
            var connectionString = Environment.GetEnvironmentVariable("AzureConnectionString", EnvironmentVariableTarget.Process);
            var blobServiceClient = new BlobServiceClient(connectionString);
            _blobContainer = blobServiceClient.GetBlobContainerClient("files");
        }
        public async Task UploadFilesAsync(IFormFile file, string userEmail)
        {
            BlobClient client = _blobContainer.GetBlobClient(file.FileName);

            await using (Stream data = file.OpenReadStream())
            {
                await client.UploadAsync(data, true);
            }

            client.SetMetadata(new Dictionary<string, string>
            {
                { "UserEmail", userEmail }
            });
        }
    }
}
