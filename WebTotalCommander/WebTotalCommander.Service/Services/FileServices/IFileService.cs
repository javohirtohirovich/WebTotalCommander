using Microsoft.AspNetCore.Http;
using System.Text;
using WebTotalCommander.Service.ViewModels.File;

namespace WebTotalCommander.Service.Services.FileServices;

public interface IFileService
{
    public Task<bool> CreateFile(FileViewModel fileView);
    public Task<bool> DeleteFile(FileDeleteViewModel fileView);
    public Task<(MemoryStream memoryStream,  string filePath)> DownloadFileAsync(string filePath);
    public Task<MemoryStream> GetTxtFileAsync(string filePath);
    public Task<bool> EditTextTxtFileAsync(string filePath, IFormFile formFile);

}
