using WebTotalCommander.DataAccess.Models;

namespace WebTotalCommander.Repository.Folders;

public interface IFolderRepository
{
    public bool CreateFolder(Folder folder);
}
