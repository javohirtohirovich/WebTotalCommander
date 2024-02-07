using Microsoft.AspNetCore.Http;
using System.Text;
using WebTotalCommander.FileAccess.Models.File;

namespace WebTotalCommander.Repository.Files;

public interface IFileRepository
{
    public Task<bool> CreateFile(FileModel file);
    public Task<bool> DeleteFile(FileDeleteModel file);
    public Task<MemoryStream> DownloadFileAsync(string filePath);
    public Task<MemoryStream> GetTxtFileAsync(string filePath);
    public Task<bool> EditTextTxtFile(string filePath, IFormFile formFile);
}
