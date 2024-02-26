using System.IO.Compression;
using WebTotalCommander.Core.Errors;
using WebTotalCommander.FileAccess.Models.Common;
using WebTotalCommander.FileAccess.Models.Folder;

namespace WebTotalCommander.Repository.Folders;

public class FolderRepository : IFolderRepository
{
    public FolderGetAllModel GetAllFolder(string path)
    {
        try
        {
            var model = new FolderGetAllModel();
            model.Files = Directory.GetFiles(path);
            model.FolderNames = Directory.GetDirectories(path);

            return model;
        }
        catch (Exception ex)
        {
            throw new FolderUnexpectedException($"Folder Unexpected error!", ex);
        }
    }

    public bool CreateFolder(Folder folder)
    {
        try
        {
            var path = Path.Combine(folder.FolderMainName, folder.FolderPath, folder.FolderName);
            var result = Directory.CreateDirectory(path);
            return true;
        }
        catch (Exception ex)
        {
            throw new FolderUnexpectedException($"Folder Unexpected error!", ex);
        }
    }

    public bool DeleteFolder(Folder folder)
    {
        try
        {
            var path = Path.Combine(folder.FolderMainName, folder.FolderPath, folder.FolderName);
            Directory.Delete(path, true);

            return true;
        }
        catch (Exception ex)
        {
            throw new FolderUnexpectedException($"Folder Unexpected error!", ex);
        }
    }

    public bool RenameFolder(FolderRename folderRename)
    {
        try
        {
            var oldPath = Path.Combine(folderRename.MainFolderName, folderRename.FolderPath, folderRename.FolderOldName);
            var newPath = Path.Combine(folderRename.MainFolderName, folderRename.FolderPath, folderRename.FolderNewName);

            Directory.Move(oldPath, newPath);

            return true;
        }
        catch (Exception ex)
        {
            throw new FolderUnexpectedException($"Folder Unexpected error!", ex);
        }
    }

    public async Task<Stream> DownloadFolderZipAsync(string folderPath)
    {
        try
        {
            var zipPath = Path.Combine(folderPath + ".zip");
            ZipFile.CreateFromDirectory(folderPath, zipPath);

            var memory = new MemoryStream();
            await using (var stream = new FileStream(zipPath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            File.Delete(zipPath);

            return memory;
        }
        catch (Exception ex)
        {
            throw new FolderUnexpectedException($"Folder Unexpected error!", ex);
        }
    }
}
