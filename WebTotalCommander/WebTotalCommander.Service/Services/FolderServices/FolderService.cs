using WebTotalCommander.Core.Errors;
using WebTotalCommander.FileAccess.Models.Folder;
using WebTotalCommander.Repository.Folders;
using WebTotalCommander.Service.ViewModels;
using WebTotalCommander.Service.ViewModels.GetAllViewModel;

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

    public async Task<FolderGetAllViewModel> FolderGetAllAsync(string folder_path, string folder_name)
    {
        string path = Path.Combine(ROOTPATH, folder_path, folder_name);
        if (!Directory.Exists(path)) { throw new EntryNotFoundException("Folder not found!"); }

        Folder folder = new Folder()
        {
            FolderName = folder_name,
            FolderPath = folder_path,
        };

        FolderGetAllModel folderGetAll = await _repository.GetAll(folder);

        FolderGetAllViewModel result = new FolderGetAllViewModel();
        for (int i = 0; i < folderGetAll.Files.Count; i++)
        {
            FileInfo fileInfo = new FileInfo(folderGetAll.Files[i]);
            FileView fileView = new FileView()
            {
                FileName = fileInfo.Name,
                FileExtension = fileInfo.Extension,
                FilePath = folderGetAll.Files[i]
            };
            result.Files.Add(fileView);
        }

        for (int i = 0;i< folderGetAll.FolderNames.Count;i++)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(folderGetAll.FolderNames[i]);

            FolderView folderView = new FolderView()
            {
                FolderName= directoryInfo.Name,
                FolderPath = folderGetAll.FolderNames[i]
            };
            result.Folders.Add(folderView);
        }
        return result;

    }

    public async Task<IList<FolderGetAllPaginationViewModel>> FolderGetAllPaginationAsync(string folder_path, string folder_name)
    {
        string path = Path.Combine(ROOTPATH, folder_path, folder_name);
        if (!Directory.Exists(path)) { throw new EntryNotFoundException("Folder not found!"); }

        Folder folder = new Folder()
        {
            FolderName = folder_name,
            FolderPath = folder_path,
        };

        FolderGetAllModel folderGetAll = await _repository.GetAll(folder);

        List<FolderGetAllPaginationViewModel> result = new List<FolderGetAllPaginationViewModel>();
        for (int i = 0; i < folderGetAll.FolderNames.Count; i++)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(folderGetAll.FolderNames[i]);

            FolderGetAllPaginationViewModel folderView = new FolderGetAllPaginationViewModel()
            {
                Name = directoryInfo.Name,
                Path = folderGetAll.FolderNames[i],
                Extension = "folder"
            };
            result.Add(folderView);
        }
        for (int i = 0; i < folderGetAll.Files.Count; i++)
        {
            FileInfo fileInfo = new FileInfo(folderGetAll.Files[i]);
            FolderGetAllPaginationViewModel fileView = new FolderGetAllPaginationViewModel()
            {
                Name = fileInfo.Name,
                Extension = fileInfo.Extension,
                Path = folderGetAll.Files[i]
            };
            result.Add(fileView);
        }
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
