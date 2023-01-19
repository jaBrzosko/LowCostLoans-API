using Contracts.Common;
using Microsoft.EntityFrameworkCore;

namespace Services.Endpoints.Helpers;

public static class PaginationHelper
{
    public async static Task<PaginationResultDto<T>> GetPaginatedResultAsync<T>(
        this IQueryable<T> query,
        GetPaginatedList request,
        CancellationToken cancellationToken)
    {
        const int minPageSize = 1;  
        const int maxPageSize = 100;
        
        int pageSize = Math.Clamp(request.PageSize, minPageSize, maxPageSize);
        int offset = request.PageNumber * pageSize;

        return new PaginationResultDto<T>()
        {
            Offset = offset,
            Results = await query
                .Skip(offset)
                .Take(pageSize)
                .ToListAsync(cancellationToken),
            TotalCount = await query.CountAsync(cancellationToken),
        };
    }
}
