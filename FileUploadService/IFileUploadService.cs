namespace HubFile.FileUploadService
{
    public interface IFileUploadService
    {
        Task<string> UploadFileAsync(IFormFile file);
        void DeleteFile(string filePath);
    }
}
