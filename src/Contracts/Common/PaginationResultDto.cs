namespace Contracts.Common;

public class PaginationResultDto<T>
{
    public List<T> Results { get; set; }
    public int Offset { get; set; }
    public int TotalCount { get; set; }
}