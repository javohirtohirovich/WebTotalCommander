using Microsoft.AspNetCore.Http;
using WebTotalCommander.Core.Errors;
using WebTotalCommander.FileAccess.Models.File;
using WebTotalCommander.Repository.Files;
using WebTotalCommander.Service.ViewModels.File;

namespace WebTotalCommander.Service.Services.FileServices;

public class FileService : IFileService
{
    private readonly IFileRepository _repository;
    private readonly string ROOTPATH = "DataFolder";


    public FileService(IFileRepository fileRepository)
    {
        this._repository = fileRepository;
    }

    public async Task<bool> CreateFile(FileViewModel fileView)
    {
        if (String.IsNullOrEmpty(fileView.FilePath))
        {
            fileView.FilePath = "";
        }
        var path = Path.Combine(ROOTPATH, fileView.FilePath, fileView.File.FileName);
        if (File.Exists(path))
        {
            throw new AlreadeExsistException("File already exsist!");
        }
        FileModel file = new FileModel()
        {
            FilePath = fileView.FilePath,
            FileSource = fileView.File
        };
        var result = await _repository.CreateFile(file);

        return result;

    }

    public async Task<bool> DeleteFile(FileDeleteViewModel fileView)
    {
        var path = Path.Combine(ROOTPATH, fileView.FilePath, fileView.FileName);
        if (!File.Exists(path)) { throw new EntryNotFoundException("File not found!"); }

        FileDeleteModel fileDeleteModel = new FileDeleteModel()
        {
            FileName = fileView.FileName,
            FilePath = fileView.FilePath
        };
        var result = await _repository.DeleteFile(fileDeleteModel);
        return result;
    }

    public async Task<(MemoryStream memoryStream, string filePath)> DownloadFileAsync(string filePath)
    {
        var path = Path.Combine(ROOTPATH, filePath);
        if (!File.Exists(path))
        {
            throw new EntryNotFoundException("File not found!");
        }

        var memory = await _repository.DownloadFileAsync(filePath);

        return (memory, path);
    }

    public async Task<bool> EditTextTxtFileAsync(string filePath, IFormFile formFile)
    {
        var path = Path.Combine(ROOTPATH, filePath);
        var fileInfo = new FileInfo(path);
        if (fileInfo.Extension != ".txt")
        {
            throw new ParameterInvalidException("File not txt!");
        }
        if (!File.Exists(path))
        {
            throw new EntryNotFoundException("File not found!");
        }

        await Task.Run(() =>
        {
            File.Delete(path);
        });

        var result=await _repository.EditTextTxtFile(filePath, formFile);
        return result;

    }

    public async Task<MemoryStream> GetTxtFileAsync(string filePath)
    {
        var path = Path.Combine(ROOTPATH, filePath);
        var fileInfo = new FileInfo(path);
        if (fileInfo.Extension != ".txt")
        {
            throw new ParameterInvalidException("File not txt!");
        }
        else if (!File.Exists(path))
        {
            throw new EntryNotFoundException("File not found!");
        }


        var result = await _repository.GetTxtFileAsync(filePath);
        return result;
    }
}
