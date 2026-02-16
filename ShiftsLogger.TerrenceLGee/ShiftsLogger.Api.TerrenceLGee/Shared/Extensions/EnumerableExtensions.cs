using ShiftsLogger.Api.TerrenceLGee.Shared.Extensions;
using ShiftsLogger.Api.TerrenceLGee.Shared.Pagination;

namespace ShiftsLogger.Api.TerrenceLGee.Shared.Extensions;

public static class EnumerableExtensions
{
    extension<T>(IEnumerable<T> source)
    {
        public PagedList<T> ToPagedList(int count, int page, int pageSize)
        {
            if (count > 0)
            {
                var items = source
                    .ToList();

                return new PagedList<T>(items, count, page, pageSize);
            }

            return new(Enumerable.Empty<T>(), 0, 0, 0);
        }
    }
}
