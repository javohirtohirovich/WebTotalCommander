using WebTotalCommander.Core.Errors;
using WebTotalCommander.FileAccess.Models.File;
using WebTotalCommander.Repository.Files;
using WebTotalCommander.Service.ViewModels;

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
        string path = Path.Combine(ROOTPATH, fileView.FilePath, fileView.File.FileName);
        if (File.Exists(path)) 
        { 
            throw new AlreadeExsistException("File already exsist!"); 
        }
        FileModel file = new FileModel()
        {
            FilePath = fileView.FilePath,
            FileSource = fileView.File
        };
        var result=await _repository.CreateFile(file);

        return result;

    }

    public async Task<bool> DeleteFile(FileDeleteViewModel fileView)
    {
        string path = Path.Combine(ROOTPATH, fileView.FilePath,fileView.FileName);
        if(!File.Exists(path)) { throw new EntryNotFoundException("File not found!"); }

        FileDeleteModel fileDeleteModel = new FileDeleteModel()
        {
            FileName = fileView.FileName,
            FilePath = fileView.FilePath
        };
        var result=await _repository.DeleteFile(fileDeleteModel);
        return result;
    }
}
