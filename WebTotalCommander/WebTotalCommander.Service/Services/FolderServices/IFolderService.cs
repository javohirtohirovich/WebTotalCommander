using WebTotalCommander.Service.ViewModels;

namespace WebTotalCommander.Service.Services.FolderServices;

public interface IFolderService
{
    public bool CreateFolder(FolderViewModel folderViewModel);
}
