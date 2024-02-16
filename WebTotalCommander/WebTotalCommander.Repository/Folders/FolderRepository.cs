using System.IO.Compression;
using WebTotalCommander.Core.Errors;
using WebTotalCommander.FileAccess.Models.Common;
using WebTotalCommander.FileAccess.Models.Folder;

namespace WebTotalCommander.Repository.Folders;

public class FolderRepository : IFolderRepository
{
    public async Task<FolderGetAllModel> GetAllFolderAsync(string path)
    {
        try
        {
            var model = new FolderGetAllModel();
            await Task.Run(() =>
            {
                model.Files = Directory.GetFiles(path);
                model.FolderNames = Directory.GetDirectories(path);
            });

            return model;
        }
        catch (Exception ex)
        {
            throw new FolderUnexpectedException($"Folder Unexpected error: {ex.Message}");
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
            throw new FolderUnexpectedException($"Folder Unexpected error: {ex.Message}");
        }
    }

    public async Task<bool> DeleteFolderAsync(Folder folder)
    {
        try
        {
            var path = Path.Combine(folder.FolderMainName, folder.FolderPath, folder.FolderName);
            await Task.Run(() =>
            {
                Directory.Delete(path, true);
            });

            return true;
        }
        catch (Exception ex)
        {
            throw new FolderUnexpectedException($"Folder Unexpected error: {ex.Message}");
        }
    }

    public async Task<bool> RenameFolderAsync(FolderRename folderRename)
    {
        try
        {
            var oldPath = Path.Combine(folderRename.MainFolderName, folderRename.FolderPath, folderRename.FolderOldName);
            var newPath = Path.Combine(folderRename.MainFolderName, folderRename.FolderPath, folderRename.FolderNewName);

            await Task.Run(() =>
            {
                Directory.Move(oldPath, newPath);

            });
            return true;
        }
        catch (Exception ex)
        {
            throw new FolderUnexpectedException($"Folder Unexpected error: {ex.Message}");
        }
    }

    public async Task<MemoryStream> DownloadFolderZipAsync(string folderPath)
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

            await Task.Run(() =>
            {
                File.Delete(zipPath);
            });

            return memory;
        }
        catch (Exception ex)
        {
            throw new FolderUnexpectedException($"Folder Unexpected error: {ex.Message}");
        }
    }
}
