using WebTotalCommander.FileAccess.Models.Common;
using WebTotalCommander.FileAccess.Models.Folder;

namespace WebTotalCommander.Repository.Folders;

public interface IFolderRepository
{
    public Task<FolderGetAllModel> GetAll(Folder folder);
    public bool CreateFolder(Folder folder);
    public bool DeleteFolder(Folder folder);
    public bool RenameFolder(FolderRename folderRename);

}
