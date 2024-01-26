
using WebTotalCommander.FileAccess.Models;

namespace WebTotalCommander.Repository.Folders;

public class FolderRepository : IFolderRepository
{
    private string ROOTPATH = "DataFolder";

    public bool CreateFolder(Folder folder)
    {
        try
        {
            string path = Path.Combine(ROOTPATH, folder.FolderPath, folder.FolderName);
            if(Directory.Exists(path)) { return false; }
            var  result = Directory.CreateDirectory(path);
            return true;
        }
        catch 
        {
            return false;
        }   
    }

    public bool DeleteFolder(Folder folder)
    {
        try
        {
            string path = Path.Combine(ROOTPATH, folder.FolderPath, folder.FolderName);
            Directory.Delete(path,true);
            return true;
        }
        catch { return false; }
    }
    public bool RenameFolder(FolderRename folderRename)
    {
        try
        {
            string oldPath = Path.Combine(ROOTPATH, folderRename.FolderPath, folderRename.FolderOldName);
            string newPath = Path.Combine(ROOTPATH, folderRename.FolderPath, folderRename.FolderNewName);
            Directory.Move(oldPath,newPath);
            return true;
        }
        catch 
        { return false; }
    }
}
