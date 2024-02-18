using WebTotalCommander.Service.ViewModels.Common;

namespace WebTotalCommander.Service.Common.Interface;

public interface IFilter
{
    public List<FolderFileViewModel> FilterFolder(FilterViewModel filter, List<FolderFileViewModel> folderList);
}
