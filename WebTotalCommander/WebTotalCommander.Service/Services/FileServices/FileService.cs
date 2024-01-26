using WebTotalCommander.Core.Errors;
using WebTotalCommander.FileAccess.Models;
using WebTotalCommander.Repository.Files;
using WebTotalCommander.Service.ViewModels;

namespace WebTotalCommander.Service.Services.FileServices;

public class FileService : IFileService
{
    private readonly IFileRepository _repository;

    public FileService(IFileRepository fileRepository) 
    {
        this._repository = fileRepository;
    }

    public async Task<bool> CreateFile(FileViewModel fileView)
    {
        FileModel file = new FileModel()
        {
            FilePath = fileView.FilePath,
            FileSource = fileView.File
        };
        var result=await _repository.CreateFile(file);

        if (result)
        {
            return result;
        }
        else
        {
            throw new AlreadeExsistException("File already exsist!");
            
        }

    }
}
