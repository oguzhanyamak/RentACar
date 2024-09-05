using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Persistence.Paging;

public static class IQueryablePaginateExtensions
{
    public static async Task<Paginate<TEntity>> ToPaginateAsync<TEntity>(this IQueryable<TEntity> source, int index, int size, CancellationToken token = default)
    {

        int count = await source.CountAsync(token).ConfigureAwait(false);
        List<TEntity> items = await source.Skip(index * size).Take(size).ToListAsync(token).ConfigureAwait(false);
        Paginate<TEntity> list = new() { Count = count, Items = items, Size = size, Index = index, Pages = (int)Math.Ceiling(count / (double)size) };
        return list;
    }

    public static  Paginate<TEntity> ToPaginate<TEntity>(this IQueryable<TEntity> source, int index, int size)
    {

        int count = source.Count();
        List<TEntity> items = source.Skip(index * size).Take(size).ToList();
        Paginate<TEntity> list = new() { Count = count, Items = items, Size = size, Index = index, Pages = (int)Math.Ceiling(count / (double)size) };
        return list;
    }
}
