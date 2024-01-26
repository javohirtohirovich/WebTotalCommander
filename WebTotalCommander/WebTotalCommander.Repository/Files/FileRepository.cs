using WebTotalCommander.FileAccess.Models.File;

namespace WebTotalCommander.Repository.Files;

public class FileRepository : IFileRepository
{
    private string ROOTPATH = "DataFolder";

    public async Task<bool> CreateFile(FileModel file)
    {
        try
        {
            string path = Path.Combine(ROOTPATH, file.FilePath, file.FileSource.FileName);
            Stream stream = new FileStream(path, FileMode.Create);
            await file.FileSource.CopyToAsync(stream);
            stream.Close();
            return true;
        }
        catch
        {
            return false;
        }
       
    }

    public async Task<bool> DeleteFile(FileDeleteModel file)
    {
        string path = Path.Combine(ROOTPATH, file.FilePath, file.FileName);
        await Task.Run(async () =>
        {
            File.Delete(path);
        });
        return true;
    }
}
