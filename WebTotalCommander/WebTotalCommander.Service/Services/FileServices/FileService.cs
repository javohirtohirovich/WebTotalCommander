using WebTotalCommander.Core.Errors;
using WebTotalCommander.FileAccess.Models.File;
using WebTotalCommander.Repository.Files;
using WebTotalCommander.Repository.Settings;
using WebTotalCommander.Service.ViewModels.File;

namespace WebTotalCommander.Service.Services.FileServices;

public class FileService : IFileService
{
    private readonly IFileRepository _repository;
    private readonly string _mainFolderName;

    public FileService(IFileRepository fileRepository, FolderSettings folderSettings)
    {
        this._repository = fileRepository;
        this._mainFolderName = folderSettings.MainFolderName;
    }

    public async Task<bool> CreateFileAsync(FileViewModel fileView)
    {
        if (String.IsNullOrEmpty(fileView.FilePath))
        {
            fileView.FilePath = "";
        }
        var path = Path.Combine(_mainFolderName, fileView.FilePath, fileView.FileName);
        if (File.Exists(path))
        {
            throw new AlreadeExsistException("File already exsist!");
        }
        FileModel file = new FileModel()
        {
            FilePath = path,
            FileSource = fileView.File
        };
        var result = await _repository.CreateFileAsync(file);

        return result;
    }

    public bool DeleteFile(FileDeleteViewModel fileView)
    {
        var path = Path.Combine(_mainFolderName, fileView.FilePath, fileView.FileName);
        if (!File.Exists(path)) { throw new EntryNotFoundException("File not found!"); }

        var result = _repository.DeleteFile(path);
        return result;
    }

    public async Task<FileDownloadViewModel> DownloadFileAsync(string filePath)
    {
        var path = Path.Combine(_mainFolderName, filePath);
        if (!File.Exists(path))
        {
            throw new EntryNotFoundException("File not found!");
        }

        var memory = await _repository.DownloadFileAsync(path);

        var fileDownloadView = new FileDownloadViewModel()
        {
            File = memory,
            FilePath = path,
        };

        return fileDownloadView;
    }

    public async Task<bool> EditTextTxtFileAsync(string filePath, Stream file)
    {
        var path = Path.Combine(_mainFolderName, filePath);
        var fileInfo = new FileInfo(path);
        if (fileInfo.Extension != ".txt")
        {
            throw new ParameterInvalidException("File not txt!");
        }
        if (!File.Exists(path))
        {
            throw new EntryNotFoundException("File not found!");
        }

        _repository.DeleteFile(path);

        var result = await _repository.EditTextTxtFileAsync(path, file);
        return result;
    }

    public async Task<Stream> GetTxtFileAsync(string filePath)
    {
        var path = Path.Combine(_mainFolderName, filePath);
        var fileInfo = new FileInfo(path);
        if (fileInfo.Extension != ".txt")
        {
            throw new ParameterInvalidException("File not txt!");
        }

        if (!File.Exists(path))
        {
            throw new EntryNotFoundException("File not found!");
        }

        var result = await _repository.GetTxtFileAsync(path);
        return result;
    }
}
