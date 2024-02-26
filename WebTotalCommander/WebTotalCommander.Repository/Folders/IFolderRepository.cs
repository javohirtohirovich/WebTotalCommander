using WebTotalCommander.FileAccess.Models.Common;
using WebTotalCommander.FileAccess.Models.Folder;

namespace WebTotalCommander.Repository.Folders;

public interface IFolderRepository
{
    public FolderGetAllModel GetAllFolder(string path);
    public bool CreateFolder(Folder folder);
    public bool DeleteFolder(Folder folder);
    public bool RenameFolder(FolderRename folderRename);
    public Task<Stream> DownloadFolderZipAsync(string path);


}
