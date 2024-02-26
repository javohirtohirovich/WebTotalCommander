using WebTotalCommander.Service.Common.Interface;
using WebTotalCommander.Service.ViewModels.Common;

namespace WebTotalCommander.Service.Common.Service;

public class Sorter : ISorter
{
    public List<FolderFileViewModel> SortAsc(FolderGetAllQuery query, List<FolderFileViewModel> folderFileViewModels)
    {
        return query.SortField switch
        {
            "name" => folderFileViewModels.OrderBy(x => x.Name).ToList(),
            "extension" => folderFileViewModels.OrderBy(x => x.Extension).ToList(),
            "createdDate" => folderFileViewModels.OrderBy(x => x.CreatedDate).ToList(),
            _ => throw new ArgumentException($"Invalid sort field: {query.SortField}")
        };
    }

    public List<FolderFileViewModel> SortDesc(FolderGetAllQuery query, List<FolderFileViewModel> folderFileViewModels)
    {
        return query.SortField switch
        {
            "name" => folderFileViewModels.OrderByDescending(x => x.Name).ToList(),
            "extension" => folderFileViewModels.OrderByDescending(x => x.Extension).ToList(),
            "createdDate" => folderFileViewModels.OrderByDescending(x => x.CreatedDate).ToList(),
            _ => throw new ArgumentException($"Invalid sort field: {query.SortField}")
        };
    }

}
