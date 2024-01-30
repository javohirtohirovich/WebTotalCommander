using Microsoft.AspNetCore.Http;

namespace WebTotalCommander.Service.ViewModels;

public class FileViewModel
{
    public string? FilePath { get; set; }=String.Empty;
    public IFormFile File { get; set; } = default!;
}
