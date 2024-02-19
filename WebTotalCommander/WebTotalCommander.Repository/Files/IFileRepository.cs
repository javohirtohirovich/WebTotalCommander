using WebTotalCommander.FileAccess.Models.File;

namespace WebTotalCommander.Repository.Files;

public interface IFileRepository
{
    public Task<bool> CreateFileAsync(FileModel file);
    public bool DeleteFile(string path);
    public Task<Stream> DownloadFileAsync(string path);
    public Task<Stream> GetTxtFileAsync(string filePath);
    public Task<bool> EditTextTxtFileAsync(string path, Stream file);
}
