using WebTotalCommander.FileAccess.Models.File;

namespace WebTotalCommander.Repository.Files;

public interface IFileRepository
{
    public Task<bool> CreateFileAsync(FileModel file);
    public Task<bool> DeleteFileAsync(string path);
    public Task<MemoryStream> DownloadFileAsync(string path);
    public Task<MemoryStream> GetTxtFileAsync(string filePath);
    public Task<bool> EditTextTxtFileAsync(string path, Stream file);
}
