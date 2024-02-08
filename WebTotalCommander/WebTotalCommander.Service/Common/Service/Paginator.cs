using WebTotalCommander.FileAccess.Utils;
using WebTotalCommander.Service.Common.Interface;

namespace WebTotalCommander.Service.Common.Service;

public class Paginator : IPaginator
{
    public PaginationMetaData Paginate(long itemsCount, PaginationParams @params)
    {
        PaginationMetaData paginationMetaData = new PaginationMetaData();

        paginationMetaData.CurrentPage = 1;
        paginationMetaData.TotalItems = (int)itemsCount;
        paginationMetaData.PageSize = 5;

        paginationMetaData.TotalPages = 10;
        paginationMetaData.HasPrevious = paginationMetaData.CurrentPage > 1;
        paginationMetaData.HasNext = paginationMetaData.CurrentPage < paginationMetaData.TotalPages;

        return paginationMetaData;
      
    }
}
