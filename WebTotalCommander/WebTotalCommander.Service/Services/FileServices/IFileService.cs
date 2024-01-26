using WebTotalCommander.Service.ViewModels;

namespace WebTotalCommander.Service.Services.FileServices;

public interface IFileService
{
    public Task<bool> CreateFile(FileViewModel fileView);

}
