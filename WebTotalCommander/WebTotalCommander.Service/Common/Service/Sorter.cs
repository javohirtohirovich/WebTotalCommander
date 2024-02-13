using System.Collections;
using WebTotalCommander.Service.Common.Interface;
using WebTotalCommander.Service.ViewModels.Common;

namespace WebTotalCommander.Service.Common.Service;

public class Sorter : ISorter
{
    public List<FolderFileViewModel> SortAsc(FolderGetAllQuery query, List<FolderFileViewModel> folderFileViewModels)
    {
        return query.SortField switch
        {
            "Name" => folderFileViewModels.OrderBy(x => x.Name).ToList(),
            "Extension" => folderFileViewModels.OrderBy(x => x.Extension).ToList(),
            _ => throw new ArgumentException($"Invalid sort field: {query.SortField}")
        };
    }

    public List<FolderFileViewModel> SortDesc(FolderGetAllQuery query, List<FolderFileViewModel> folderFileViewModels)
    {
        return query.SortField switch
        {
            "Name" => folderFileViewModels.OrderByDescending(x => x.Name).ToList(),
            "Extension" => folderFileViewModels.OrderByDescending(x => x.Extension).ToList(),
            _ => throw new ArgumentException($"Invalid sort field: {query.SortField}")
        };
    }

}
