using Microsoft.AspNetCore.Http;

namespace WebTotalCommander.FileAccess.Models.Folder;

public class FolderGetAllModel
{
    public IList<string> FolderNames { get; set; }= new List<string>();
    public IList<string> Files { get; set; } = new List<string>();
}
