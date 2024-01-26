
using WebTotalCommander.FileAccess.Models;

namespace WebTotalCommander.Repository.Folders;

public interface IFolderRepository
{
    public bool CreateFolder(Folder folder);
}
