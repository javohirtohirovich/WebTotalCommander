using Microsoft.AspNetCore.Http;

namespace WebTotalCommander.FileAccess.Models;


public class FileModel
{
    public string FilePath { get; set; } = String.Empty;
    public IFormFile FileSource { get; set; } = default!;
}

