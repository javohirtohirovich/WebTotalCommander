using Microsoft.AspNetCore.Http;

namespace WebTotalCommander.Service.ViewModels.File;

public class FileViewModel
{
    public string? FilePath { get; set; } = string.Empty;
    public IFormFile File { get; set; } = default!;
}
