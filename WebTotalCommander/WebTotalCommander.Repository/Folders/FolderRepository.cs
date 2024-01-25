using WebTotalCommander.DataAccess.Models;

namespace WebTotalCommander.Repository.Folders;

public class FolderRepository : IFolderRepository
{
    private string ROOTPATH = "WebTotalCommander\\WebTotalCommander.DataAccess\\Databse";

    public bool CreateFolder(Folder folder)
    {
        try
        {
            string path = Path.Combine(ROOTPATH, folder.FolderPath, folder.FolderName);
            var  result = Directory.CreateDirectory(path);
            return true;
        }
        catch 
        {
            return false;
        }   
    }
}
