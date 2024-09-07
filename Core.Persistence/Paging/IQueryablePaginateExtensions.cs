using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Persistence.Paging;

public static class IQueryablePaginateExtensions
{
    public static async Task<Paginate<T>> ToPaginateAsync<T>(
            this IQueryable<T> source,
            int index,
            int size,
            int from = 0,
            CancellationToken cancellationToken = default
        )
    {
        if (from > index)
            throw new ArgumentException($"From: {from} > Index: {index}, must from <= Index");

        int count = await source.CountAsync(cancellationToken).ConfigureAwait(false);

        List<T> items = await source.Skip((index - from) * size).Take(size).ToListAsync(cancellationToken).ConfigureAwait(false);

        Paginate<T> list =
            new()
            {
                Index = index,
                Size = size,
                From = from,
                Count = count,
                Items = items,
                Pages = (int)Math.Ceiling(count / (double)size)
            };
        return list;
    }


    /*
        Fonksiyon Ne Yapar?
        Asenkron olarak belirli bir sayfaya ait verileri getirir (Skip, Take ile).
        source üzerindeki toplam öğe sayısını bulur (CountAsync()).
        Sayfa numarası, sayfa boyutu, toplam öğe sayısı ve toplam sayfa sayısı gibi bilgileri içeren bir Paginate<T> nesnesi oluşturur.
        Sonuçta, belirli bir sayfaya ait verileri içeren ve sayfalama bilgileri ile donatılmış bir Paginate<T> nesnesi döner.
        Bu fonksiyon, özellikle büyük veri kümelerinde, veritabanından sayfalama yaparak verimli veri çekmek için kullanılabilir.
--------------
        ConfigureAwait(false), bu varsayılan davranışı değiştirmemizi sağlar ve işlemin tamamlanmasının ardından aynı bağlama geri dönmeyi gereksiz hale getirir. 
        Yani, işlemin tamamlanmasından sonra mevcut iş parçacığına veya bağlama bağlı kalınmaksızın bir başka uygun iş parçacığı kullanılarak işlem tamamlanır.
        Bu, genellikle UI dışı uygulamalarda performans artışı sağlar, çünkü bağlamı korumak maliyetlidir.
    */

    public static Paginate<T> ToPaginate<T>(this IQueryable<T> source, int index, int size, int from = 0)
    {
        if (from > index)
            throw new ArgumentException($"From: {from} > Index: {index}, must from <= Index");

        int count = source.Count();
        var items = source.Skip((index - from) * size).Take(size).ToList();

        Paginate<T> list =
            new()
            {
                Index = index,
                Size = size,
                From = from,
                Count = count,
                Items = items,
                Pages = (int)Math.Ceiling(count / (double)size)
            };
        return list;
    }
}
