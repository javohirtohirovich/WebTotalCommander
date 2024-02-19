using WebTotalCommander.Service.ViewModels.File;

namespace WebTotalCommander.Service.Services.FileServices;

public interface IFileService
{
    public Task<bool> CreateFileAsync(FileViewModel fileView, string mainFolderName);
    public bool DeleteFile(FileDeleteViewModel fileView, string mainFolderName);
    public Task<FileDownloadViewModel> DownloadFileAsync(string filePath, string mainFolderName);
    public Task<Stream> GetTxtFileAsync(string filePath, string mainFolderName);
    public Task<bool> EditTextTxtFileAsync(string filePath, Stream formFile, string mainFolderName);

}
