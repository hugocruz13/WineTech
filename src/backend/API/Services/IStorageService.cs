namespace API.Services
{
    public interface IStorageService
    {
        Task<string> UploadFileAsync(IFormFile file, string containerName);
    }
}
