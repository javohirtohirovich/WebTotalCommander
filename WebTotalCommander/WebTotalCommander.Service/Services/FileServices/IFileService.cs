using WebTotalCommander.Service.ViewModels.File;

namespace WebTotalCommander.Service.Services.FileServices;

public interface IFileService
{
    public Task<bool> CreateFileAsync(FileViewModel fileView);
    public Task<bool> DeleteFileAsync(FileDeleteViewModel fileView);
    public Task<(MemoryStream memoryStream, string filePath)> DownloadFileAsync(string filePath);
    public Task<MemoryStream> GetTxtFileAsync(string filePath);
    public Task<bool> EditTextTxtFileAsync(string filePath, Stream formFile);

}
