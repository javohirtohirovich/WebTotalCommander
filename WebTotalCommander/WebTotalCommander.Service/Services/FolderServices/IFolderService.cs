using WebTotalCommander.Service.ViewModels.Common;
using WebTotalCommander.Service.ViewModels.Folder;

namespace WebTotalCommander.Service.Services.FolderServices;

public interface IFolderService
{
    public Task<FolderGetAllViewModel> FolderGetAllAsync(FolderGetAllQuery query);
    public bool CreateFolder(FolderViewModel folderViewModel);
    public Task<bool> DeleteFolderAsync(FolderViewModel folderViewModel);
    public Task<bool> RenameFolderAsync(FolderRenameViewModel folderRenameViewModel);
    public Task<(MemoryStream memoryStream, string fileName)> DownloadFolderZipAsync(string folderPath, string folderName);
}
