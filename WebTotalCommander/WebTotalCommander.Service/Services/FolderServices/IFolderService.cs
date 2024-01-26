using WebTotalCommander.Service.ViewModels;

namespace WebTotalCommander.Service.Services.FolderServices;

public interface IFolderService
{
    public bool CreateFolder(FolderViewModel folderViewModel);
    public bool DeleteFolder(FolderViewModel folderViewModel);
    public bool RenameFolder(FolderRenameViewModel folderRenameViewModel);
}
