using WebTotalCommander.FileAccess.Models;

namespace WebTotalCommander.Repository.Files;

public interface IFileRepository
{
    public Task<bool> CreateFile(FileModel file);
}
