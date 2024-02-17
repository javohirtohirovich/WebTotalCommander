using WebTotalCommander.Service.Common.Interface;
using WebTotalCommander.Service.ViewModels.Common;

namespace WebTotalCommander.Service.Common.Service;

public class Filter : IFilter
{
    public List<FolderFileViewModel> FilterFolder(FolderGetAllQuery query, List<FolderFileViewModel> folderList)
    {
        foreach (var item in query.Filter.Filters)
        {
            var isName = item.Filters.Any(x => x.Field == "name");
            if (isName)
            {
                var containsFilter = item.Filters.FirstOrDefault(x => x.Operator == "contains");
                if (containsFilter != null)
                {
                    folderList = folderList.Where(x => x.Name.Contains(containsFilter.Value)).ToList();
                }
            }
            var isExtension = item.Filters.Any(x => x.Field == "extension");
            if (isExtension)
            {
                var containsFilter = item.Filters.FirstOrDefault(x => x.Operator == "contains");
                if (containsFilter != null)
                {
                    folderList = folderList.Where(x => x.Extension.Contains(containsFilter.Value)).ToList();
                }
            }

            var isCreateDate = item.Filters.Any(x => x.Field == "createdDate");
            if (isCreateDate)
            {
                var containsFilterEq = item.Filters.FirstOrDefault(x => x.Operator == "eq");
                if (containsFilterEq != null)
                {
                    folderList = folderList.Where(x => x.CreatedDate == DateTime.Parse(containsFilterEq.Value)).ToList();
                }
                var containsFilterGte = item.Filters.FirstOrDefault(x => x.Operator == "gte");
                if (containsFilterGte != null)
                {
                    folderList = folderList.Where(x => x.CreatedDate >= DateTime.Parse(containsFilterGte.Value)).ToList();
                }
                var containsFilterLte = item.Filters.FirstOrDefault(x => x.Operator == "lte");
                if (containsFilterLte != null)
                {
                    folderList = folderList.Where(x => x.CreatedDate <= DateTime.Parse(containsFilterLte.Value)).ToList();
                }
                var containsFilterGt = item.Filters.FirstOrDefault(x => x.Operator == "gt");
                if (containsFilterGt != null)
                {
                    folderList = folderList.Where(x => x.CreatedDate > DateTime.Parse(containsFilterGt.Value)).ToList();
                }
                var containsFilterLt = item.Filters.FirstOrDefault(x => x.Operator == "lt");
                if (containsFilterLt != null)
                {
                    folderList = folderList.Where(x => x.CreatedDate < DateTime.Parse(containsFilterLt.Value)).ToList();
                }
            }
        }
        return folderList;
    }
}
