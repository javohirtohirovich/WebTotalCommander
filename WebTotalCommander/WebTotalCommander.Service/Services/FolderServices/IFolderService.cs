using WebTotalCommander.Service.ViewModels;
using WebTotalCommander.Service.ViewModels.GetAllViewModel;

namespace WebTotalCommander.Service.Services.FolderServices;

public interface IFolderService
{
    public Task<FolderGetAllViewModel> FolderGetAllAsync(string folder_path, string folder_name);
    public bool CreateFolder(FolderViewModel folderViewModel);
    public bool DeleteFolder(FolderViewModel folderViewModel);
    public bool RenameFolder(FolderRenameViewModel folderRenameViewModel);
}
