using WebTotalCommander.FileAccess.Utils;
using WebTotalCommander.Service.ViewModels.Common;
using WebTotalCommander.Service.ViewModels.Folder;

namespace WebTotalCommander.Service.Services.FolderServices;

public interface IFolderService
{
    public Task<FolderGetAllViewModel> FolderGetAllAsync(string folderPath,PaginationParams @params);
    public bool CreateFolder(FolderViewModel folderViewModel);
    public bool DeleteFolder(FolderViewModel folderViewModel);
    public bool RenameFolder(FolderRenameViewModel folderRenameViewModel);
    public Task<(MemoryStream memoryStream, string fileName)> DownloadFolderZipAsync(string folderPath,string folderName);
}
