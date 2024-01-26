using Microsoft.AspNetCore.Http;

namespace WebTotalCommander.FileAccess.Models.File;


public class FileModel
{
    public string FilePath { get; set; } = string.Empty;
    public IFormFile FileSource { get; set; } = default!;
}

