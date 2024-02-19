using WebTotalCommander.Service.ViewModels.Common;
using WebTotalCommander.Service.ViewModels.Folder;

namespace WebTotalCommander.Service.Services.FolderServices;

public interface IFolderService
{
    public FolderGetAllViewModel FolderGetAll(FolderGetAllQuery query, string mainFolderName);
    public bool CreateFolder(FolderViewModel folderViewModel, string mainFolderName);
    public bool DeleteFolder(FolderViewModel folderViewModel, string mainFolderName);
    public bool RenameFolder(FolderRenameViewModel folderRenameViewModel, string mainFolderName);
    public Task<(Stream memoryStream, string fileName)> DownloadFolderZipAsync(string folderPath, string folderName, string mainFolderName);
}
