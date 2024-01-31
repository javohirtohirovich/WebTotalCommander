namespace WebTotalCommander.Service.ViewModels.GetAllViewModel;

public class FolderGetAllViewModel
{
    public IList<FolderView> Folders { get; set; } = new List<FolderView>();
    public IList<FileView> Files { get; set; }= new List<FileView>();
}
