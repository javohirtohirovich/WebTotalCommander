using WebTotalCommander.Service.Common.Interface;
using WebTotalCommander.Service.ViewModels.Common;

namespace WebTotalCommander.Service.Common.Service;

public class Filter : IFilter
{
    public List<FolderFileViewModel> FilterFolder(FilterViewModel filter, List<FolderFileViewModel> folderList)
    {
        var folderFilterColumns = new List<List<FolderFileViewModel>>();
        for (var i = 0; i < filter.Filters.Count; i++)
        {
            var columnName = filter.Filters[i].Filters[0].Field;
            if (!String.IsNullOrEmpty(columnName))
            {
                switch (columnName)
                {
                    case "name":
                        folderFilterColumns.Add(FilterNameColumn(filter.Filters[i], folderList));

                        break;

                    case "extension":
                        folderFilterColumns.Add(FilterExtensionColumn(filter.Filters[i], folderList));

                        break;

                    case "createdDate":
                        folderFilterColumns.Add(FilterCreatedDateColumn(filter.Filters[i], folderList));

                        break;
                }
            }
        }

        var folderFilterResult = new List<FolderFileViewModel>();
        folderFilterResult = folderFilterColumns[0].Slice(0, folderFilterColumns[0].Count);
        if (folderFilterColumns.Count > 1)
        {
            for (var i = 1; i < folderFilterColumns.Count; i++)
            {
                folderFilterResult = folderFilterResult.Intersect(folderFilterColumns[i]).ToList();
            }
        }

        return folderFilterResult;
    }

    private static List<FolderFileViewModel> FilterNameColumn(SubFilterViewModel filter, List<FolderFileViewModel> folderList)
    {
        var folderFilterListByName = new List<List<FolderFileViewModel>>();
        foreach (var item in filter.Filters)
        {
            switch (item.Operator)
            {
                case "contains":
                    folderFilterListByName.Add(folderList.Where(x => x.Name.Contains(item.Value)).ToList());
                    break;
                case "doesnotcontain":
                    folderFilterListByName.Add(folderList.Where(x => !x.Name.Contains(item.Value)).ToList());
                    break;
                case "eq":
                    folderFilterListByName.Add(folderList.Where(x => x.Name == item.Value).ToList());
                    break;
                case "neq":
                    folderFilterListByName.Add(folderList.Where(x => x.Name != item.Value).ToList());
                    break;
                case "startswith":
                    folderFilterListByName.Add(folderList.Where(x => x.Name.StartsWith(item.Value)).ToList());
                    break;
                case "endswith":
                    folderFilterListByName.Add(folderList.Where(x => x.Name.EndsWith(item.Value)).ToList());
                    break;
            }
        }
        if (folderFilterListByName.Count > 1)
        {
            if (filter.Logic == "and")
            {
                if (folderFilterListByName[0].Intersect(folderFilterListByName[1]).ToList().Count > 0)
                {
                    return folderFilterListByName[0].Intersect(folderFilterListByName[1]).ToList();
                }
                else
                {
                    return folderFilterListByName[0];
                }

            }
            else
            {
                return folderFilterListByName[0].Concat(folderFilterListByName[1]).Distinct().ToList();
            }
        }
        else
        {
            return folderFilterListByName[0];
        }

    }

    private static List<FolderFileViewModel> FilterExtensionColumn(SubFilterViewModel filter, List<FolderFileViewModel> folderList)
    {
        var folderFilterListByExtension = new List<List<FolderFileViewModel>>();
        foreach (var item in filter.Filters)
        {
            switch (item.Operator)
            {
                case "contains":
                    folderFilterListByExtension.Add(folderList.Where(x => x.Extension.Contains(item.Value)).ToList());
                    break;
                case "doesnotcontain":
                    folderFilterListByExtension.Add(folderList.Where(x => !x.Extension.Contains(item.Value)).ToList());
                    break;
                case "eq":
                    folderFilterListByExtension.Add(folderList.Where(x => x.Extension == item.Value).ToList());
                    break;
                case "neq":
                    folderFilterListByExtension.Add(folderList.Where(x => x.Extension != item.Value).ToList());
                    break;
                case "startswith":
                    folderFilterListByExtension.Add(folderList.Where(x => x.Extension.StartsWith(item.Value)).ToList());
                    break;
                case "endswith":
                    folderFilterListByExtension.Add(folderList.Where(x => x.Extension.EndsWith(item.Value)).ToList());
                    break;
            }
        }
        if (folderFilterListByExtension.Count > 1)
        {
            if (filter.Logic == "and")
            {
                if (folderFilterListByExtension[0].Intersect(folderFilterListByExtension[1]).ToList().Count > 0)
                {
                    return folderFilterListByExtension[0].Intersect(folderFilterListByExtension[1]).ToList();
                }
                else
                {
                    return folderFilterListByExtension[0];
                }
            }
            else
            {
                return folderFilterListByExtension[0].Concat(folderFilterListByExtension[1]).Distinct().ToList();
            }
        }
        else
        {
            return folderFilterListByExtension[0];
        }
    }

    private static List<FolderFileViewModel> FilterCreatedDateColumn(SubFilterViewModel filter, List<FolderFileViewModel> folderList)
    {
        var folderFilterListByCreatedDate = new List<List<FolderFileViewModel>>();
        foreach (var item in filter.Filters)
        {
            switch (item.Operator)
            {
                case "eq":
                    folderFilterListByCreatedDate.Add(folderList.Where(x => x.CreatedDate.Equals(item.Value)).ToList());
                    break;
                case "neq":
                    folderFilterListByCreatedDate.Add(folderList.Where(x => !x.CreatedDate.Equals(item.Value)).ToList());
                    break;
                case "gte":
                    folderFilterListByCreatedDate.Add(folderList.Where(x => x.CreatedDate >= DateTime.Parse(item.Value)).ToList());
                    break;
                case "gt":
                    folderFilterListByCreatedDate.Add(folderList.Where(x => x.CreatedDate > DateTime.Parse(item.Value)).ToList());
                    break;
                case "lte":
                    folderFilterListByCreatedDate.Add(folderList.Where(x => x.CreatedDate <= DateTime.Parse(item.Value)).ToList());
                    break;
                case "lt":
                    folderFilterListByCreatedDate.Add(folderList.Where(x => x.CreatedDate < DateTime.Parse(item.Value)).ToList());
                    break;
            }
        }
        if (folderFilterListByCreatedDate.Count > 1)
        {
            if (filter.Logic == "and")
            {
                if (folderFilterListByCreatedDate[0].Intersect(folderFilterListByCreatedDate[1]).ToList().Count > 0)
                {
                    return folderFilterListByCreatedDate[0].Intersect(folderFilterListByCreatedDate[1]).ToList();
                }
                else
                {
                    return folderFilterListByCreatedDate[0];
                }
            }
            else
            {
                return folderFilterListByCreatedDate[0].Concat(folderFilterListByCreatedDate[1]).Distinct().ToList();
            }
        }
        else
        {
            return folderFilterListByCreatedDate[0];
        }
    }

}
