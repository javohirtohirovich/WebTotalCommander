using WebTotalCommander.Core.Errors;
using WebTotalCommander.FileAccess.Models.File;

namespace WebTotalCommander.Repository.Files;

public class FileRepository : IFileRepository
{
    public async Task<bool> CreateFileAsync(FileModel file)
    {
        try
        {
            using (var stream = new FileStream(file.FilePath, FileMode.Create))
            {
                await file.FileSource.CopyToAsync(stream);
                stream.Close();
            }
            return true;
        }
        catch (Exception ex)
        {
            throw new FileUnexpectedException($"File Unexpected error!", ex);
        }
    }

    public bool DeleteFile(string path)
    {
        try
        {
            File.Delete(path);

            return true;
        }
        catch (Exception ex)
        {
            throw new FileUnexpectedException($"File Unexpected error!.", ex);
        }
    }

    public async Task<Stream> DownloadFileAsync(string path)
    {
        try
        {
            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return memory;
        }
        catch (Exception ex)
        {
            throw new FileUnexpectedException($"File Unexpected error!", ex);
        }
    }

    public async Task<bool> EditTextTxtFileAsync(string path, Stream file)
    {
        try
        {
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
                stream.Close();
            }
            return true;
        }
        catch (Exception ex)
        {
            throw new FileUnexpectedException($"File Unexpected error!", ex);
        }
    }

    public async Task<Stream> GetTxtFileAsync(string path)
    {
        try
        {
            var memory = new MemoryStream();
            await using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return memory;
        }
        catch (Exception ex)
        {
            throw new FileUnexpectedException($"File Unexpected error!", ex);
        }
    }
}
