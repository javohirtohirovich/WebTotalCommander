using System.IO.Compression;
using WebTotalCommander.FileAccess.Models.Common;
using WebTotalCommander.FileAccess.Models.Folder;

namespace WebTotalCommander.Repository.Folders;

public class FolderRepository : IFolderRepository
{
    private string ROOTPATH = "DataFolder";

    public async Task<FolderGetAllModel> GetAllFolder(string folderPath)
    {
        var path = Path.Combine(ROOTPATH, folderPath);
        var model = new FolderGetAllModel();
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
            var path = Path.Combine(ROOTPATH, folder.FolderPath, folder.FolderName);
            var result = Directory.CreateDirectory(path);
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
            var path = Path.Combine(ROOTPATH, folder.FolderPath, folder.FolderName);
            Directory.Delete(path, true);
            return true;
        }
        catch { return false; }
    }

    public bool RenameFolder(FolderRename folderRename)
    {
        try
        {
            var oldPath = Path.Combine(ROOTPATH, folderRename.FolderPath, folderRename.FolderOldName);
            var newPath = Path.Combine(ROOTPATH, folderRename.FolderPath, folderRename.FolderNewName);
            Directory.Move(oldPath, newPath);

            return true;
        }
        catch
        { return false; }
    }

    public async Task<MemoryStream> DownloadFolderZipAsync(string folderPath, string folderName)
    {
        var zipPath = Path.Combine(ROOTPATH, folderPath, folderName + ".zip");
        var path = Path.Combine(ROOTPATH, folderPath, folderName);
        ZipFile.CreateFromDirectory(path, zipPath);

        var memory = new MemoryStream();
        await using (var stream = new FileStream(zipPath, FileMode.Open))
        {
            await stream.CopyToAsync(memory);
        }
        memory.Position = 0;
        await Task.Run(() =>
        {
            File.Delete(zipPath);
        });
        return memory;
    }
}
