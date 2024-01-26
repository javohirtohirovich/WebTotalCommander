using WebTotalCommander.FileAccess.Models.File;

namespace WebTotalCommander.Repository.Files;

public interface IFileRepository
{
    public Task<bool> CreateFile(FileModel file);
    public Task<bool> DeleteFile(FileDeleteModel file);
}
