using WebTotalCommander.FileAccess.Models.Common;
using WebTotalCommander.FileAccess.Models.Folder;

namespace WebTotalCommander.Repository.Folders;

public interface IFolderRepository
{
    public Task<FolderGetAllModel> GetAllFolderAsync(string path);
    public bool CreateFolder(Folder folder);
    public Task<bool> DeleteFolderAsync(Folder folder);
    public Task<bool> RenameFolderAsync(FolderRename folderRename);
    public Task<MemoryStream> DownloadFolderZipAsync(string path);


}
