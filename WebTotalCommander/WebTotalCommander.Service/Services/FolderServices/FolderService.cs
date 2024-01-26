using WebTotalCommander.Core.Errors;
using WebTotalCommander.FileAccess.Models.Folder;
using WebTotalCommander.Repository.Folders;
using WebTotalCommander.Service.ViewModels;

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
