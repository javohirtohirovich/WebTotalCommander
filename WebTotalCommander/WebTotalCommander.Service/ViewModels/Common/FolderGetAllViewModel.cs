using WebTotalCommander.FileAccess.Utils;

namespace WebTotalCommander.Service.ViewModels.Common;

public class FolderGetAllViewModel
{
    public List<FolderFileViewModel> FolderFile { get; set; } = new List<FolderFileViewModel>();
    public PaginationMetaData PaginationMetaData { get; set; } = new PaginationMetaData();
}
