using WebTotalCommander.Service.ViewModels.Common;

namespace WebTotalCommander.Service.Common.Interface;

public interface ISorter
{
    public List<FolderFileViewModel> SortDesc(FolderGetAllQuery query, List<FolderFileViewModel> folderFileViewModels);
    public List<FolderFileViewModel> SortAsc(FolderGetAllQuery query, List<FolderFileViewModel> folderFileViewModels);

}
