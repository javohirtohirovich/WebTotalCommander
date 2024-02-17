using WebTotalCommander.Service.ViewModels.Common;

namespace WebTotalCommander.Service.Common.Interface;

public interface IFilter
{
    public List<FolderFileViewModel> FilterFolder(FolderGetAllQuery query,List<FolderFileViewModel> folderList);
}
