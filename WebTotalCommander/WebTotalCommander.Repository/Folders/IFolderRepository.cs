using WebTotalCommander.FileAccess.Models.Common;
using WebTotalCommander.FileAccess.Models.Folder;
using WebTotalCommander.FileAccess.Utils;

namespace WebTotalCommander.Repository.Folders;

public interface IFolderRepository
{
    public Task<FolderGetAllModel> GetAllFolder(string folderPath);
    public bool CreateFolder(Folder folder);
    public bool DeleteFolder(Folder folder);
    public bool RenameFolder(FolderRename folderRename);
    public Task<MemoryStream> DownloadFolderZipAsync(string folderPath, string folderName);


}
