using WebTotalCommander.FileAccess.Models;

namespace WebTotalCommander.Repository.Files;

public class FileRepository : IFileRepository
{
    private string ROOTPATH = "DataFolder";

    public async Task<bool> CreateFile(FileModel file)
    {
        try
        {
            string path = Path.Combine(ROOTPATH, file.FilePath, file.FileSource.FileName);
            if (File.Exists(path)) { return false; }
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
}
