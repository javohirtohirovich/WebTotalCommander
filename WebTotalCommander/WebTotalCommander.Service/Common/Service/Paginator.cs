using WebTotalCommander.FileAccess.Utils;
using WebTotalCommander.Service.Common.Interface;

namespace WebTotalCommander.Service.Common.Service;

public class Paginator : IPaginator
{
    public PaginationMetaData Paginate(long itemsCount, PaginationParams @params)
    {
        PaginationMetaData paginationMetaData = new PaginationMetaData();
        paginationMetaData.CurrentPage =(int) Math.Ceiling((double)@params.Skip / (double)@params.Take) + 1;
        paginationMetaData.TotalItems = (int)itemsCount;
        paginationMetaData.PageSize = @params.Take;
        paginationMetaData.TotalPages = (int)Math.Ceiling((double)itemsCount/@params.Take);
        paginationMetaData.HasPrevious = paginationMetaData.CurrentPage > 1;
        paginationMetaData.HasNext = paginationMetaData.CurrentPage < paginationMetaData.TotalPages;

        return paginationMetaData;     
    }
}
