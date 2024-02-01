using WebTotalCommander.Service.ViewModels.Common;
using WebTotalCommander.Service.ViewModels.Folder;

namespace WebTotalCommander.Service.Services.FolderServices;

public interface IFolderService
{
    public Task<IList<FolderGetAllViewModel>> FolderGetAllAsync(string folder_path,string folder_name);
    public bool CreateFolder(FolderViewModel folderViewModel);
    public bool DeleteFolder(FolderViewModel folderViewModel);
    public bool RenameFolder(FolderRenameViewModel folderRenameViewModel);
}
