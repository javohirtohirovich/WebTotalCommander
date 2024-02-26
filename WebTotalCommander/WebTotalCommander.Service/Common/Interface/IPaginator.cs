using WebTotalCommander.FileAccess.Utils;

namespace WebTotalCommander.Service.Common.Interface;

public interface IPaginator
{
    public PaginationMetaData Paginate(long itemsCount, PaginationParams @params);

}
