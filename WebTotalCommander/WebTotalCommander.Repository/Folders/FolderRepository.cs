using WebTotalCommander.FileAccess.Models.Common;
using WebTotalCommander.FileAccess.Models.Folder;

namespace WebTotalCommander.Repository.Folders;

public class FolderRepository : IFolderRepository
{
    private string ROOTPATH = "DataFolder";

    public async Task<FolderGetAllModel> GetAll(Folder folder)
    {
        string path = Path.Combine(ROOTPATH, folder.FolderPath, folder.FolderName);
        FolderGetAllModel model = new FolderGetAllModel();
        await Task.Run(() =>
        {
            model.Files = Directory.GetFiles(path);
            model.FolderNames = Directory.GetDirectories(path);
        });
       
        return model;  
    }
    
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
