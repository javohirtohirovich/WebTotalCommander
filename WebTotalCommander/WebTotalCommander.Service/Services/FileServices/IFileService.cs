using System.Text;
using WebTotalCommander.Service.ViewModels.File;

namespace WebTotalCommander.Service.Services.FileServices;

public interface IFileService
{
    public Task<bool> CreateFile(FileViewModel fileView);
    public Task<bool> DeleteFile(FileDeleteViewModel fileView);
    public Task<(MemoryStream memoryStream,  string filePath)> DownloadFileAsync(string filePath);
    public Task<string> GetTextTxtFileAsync(string filePath);

}
