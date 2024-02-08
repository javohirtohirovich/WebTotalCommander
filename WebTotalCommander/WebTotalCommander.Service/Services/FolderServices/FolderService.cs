using WebTotalCommander.Core.Errors;
using WebTotalCommander.FileAccess.Models.Common;
using WebTotalCommander.FileAccess.Models.Folder;
using WebTotalCommander.FileAccess.Utils;
using WebTotalCommander.Repository.Folders;
using WebTotalCommander.Service.Common.Service;
using WebTotalCommander.Service.ViewModels.Common;
using WebTotalCommander.Service.ViewModels.Folder;

namespace WebTotalCommander.Service.Services.FolderServices;

public class FolderService : IFolderService
{
    private readonly IFolderRepository _repository;
    private readonly string ROOTPATH = "DataFolder";

    public FolderService(IFolderRepository folderRepository)
    {
        this._repository = folderRepository;
    }
    public bool CreateFolder(FolderViewModel folderViewModel)
    {
        string path = Path.Combine(ROOTPATH, folderViewModel.FolderPath, folderViewModel.FolderName);
        if (Directory.Exists(path)) { throw new AlreadeExsistException("Folder alreade exsist!"); }

        Folder folder = new Folder()
        {
            FolderName = folderViewModel.FolderName,
            FolderPath = folderViewModel.FolderPath,
        };

        bool result=_repository.CreateFolder(folder);
        return result;
    }

    public bool DeleteFolder(FolderViewModel folderViewModel)
    {

        string path = Path.Combine(ROOTPATH, folderViewModel.FolderPath, folderViewModel.FolderName);
        if (!Directory.Exists(path)) {  throw new EntryNotFoundException("Folder not found!"); }
        Folder folder = new Folder()
        {
            FolderName = folderViewModel.FolderName,
            FolderPath = folderViewModel.FolderPath,
        };

        bool result=_repository.DeleteFolder(folder);

        return result;
    }

    public async Task<(MemoryStream memoryStream, string fileName)> DownloadFolderZipAsync(string folderPath, string folderName)
    {
        string path = Path.Combine(ROOTPATH, folderPath,folderName);
        if (!Directory.Exists(path))
        {
            throw new EntryNotFoundException("Folder not found!");
        }

        var memory = await _repository.DownloadFolderZipAsync(folderPath, folderName);
        return (memory, folderName);
    }

    public async Task<FolderGetAllViewModel> FolderGetAllAsync(string folderPath,PaginationParams @params)
    {
        string path = Path.Combine(ROOTPATH, folderPath);
        if (!Directory.Exists(path)) { throw new EntryNotFoundException("Folder not found!"); }


        FolderGetAllModel folderGetAll = await _repository.GetAllFolder(folderPath);

        FolderGetAllViewModel result = new FolderGetAllViewModel();
        for (int i = 0; i < folderGetAll.FolderNames.Count; i++)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(folderGetAll.FolderNames[i]);

            FolderFileViewModel folderView = new FolderFileViewModel()
            {
                Name = directoryInfo.Name,
                Path = folderGetAll.FolderNames[i],
                Extension = "folder"
            };
            result.FolderFile.Add(folderView);
        }
        for (int i = 0; i < folderGetAll.Files.Count; i++)
        {
            FileInfo fileInfo = new FileInfo(folderGetAll.Files[i]);
            FolderFileViewModel fileView = new FolderFileViewModel()
            {
                Name = fileInfo.Name,
                Extension = fileInfo.Extension,
                Path = folderGetAll.Files[i]
            };
            result.FolderFile .Add(fileView);
        }
        Paginator paginator = new Paginator();
        PaginationMetaData paginationMetaData = paginator.Paginate(result.FolderFile.Count, @params);
        result.PaginationMetaData = paginationMetaData;
        result.FolderFile =result.FolderFile.Skip(@params.Skip).Take(@params.Take).ToList();
        return result;

    }

    public bool RenameFolder(FolderRenameViewModel folderRenameViewModel)
    {
        string path = Path.Combine(ROOTPATH, folderRenameViewModel.FolderPath, folderRenameViewModel.FolderOldName);
        if (!Directory.Exists(path)) { throw new EntryNotFoundException("Folder not found!"); }
        FolderRename folderRename = new FolderRename()
        {
            FolderPath = folderRenameViewModel.FolderPath,
            FolderNewName = folderRenameViewModel.FolderNewName,
            FolderOldName = folderRenameViewModel.FolderOldName
        };
        bool result=_repository.RenameFolder(folderRename);
        return result;
    }
}
