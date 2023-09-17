namespace EmailWebApi.Interfaces
{
    public interface IAzureBlobService
    {
        Task UploadFilesAsync(IFormFile file, string userEmail);
    }
}
