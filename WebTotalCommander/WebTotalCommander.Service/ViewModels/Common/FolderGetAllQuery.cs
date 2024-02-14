namespace WebTotalCommander.Service.ViewModels.Common;

public class FolderGetAllQuery
{
    public string Path { get; set; } = String.Empty;
    public int Limit {  get; set; }
    public int Offset { get; set; }
    public FilterViewModel? Filter { get; set; } = null;
    public string SortField { get; set; }=String.Empty;
    public string SortDir { get; set; } = String.Empty;
}
