using WebTotalCommander.Service.ViewModels.Common;
using WebTotalCommander.Service.ViewModels.Folder;

namespace WebTotalCommander.Service.Services.FolderServices;

public interface IFolderService
{
    public FolderGetAllViewModel FolderGetAll(FolderGetAllQuery query);
    public bool CreateFolder(FolderViewModel folderViewModel);
    public bool DeleteFolder(FolderViewModel folderViewModel);
    public bool RenameFolder(FolderRenameViewModel folderRenameViewModel);
    public Task<(Stream memoryStream, string fileName)> DownloadFolderZipAsync(string folderPath, string folderName);
}
