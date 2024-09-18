using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Core.Application.Pipelines.Caching;

public class CacheRemovingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>, ICacheRemoverRequest
{

    private readonly IDistributedCache _distributedCache;

    public CacheRemovingBehavior(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request.BypassCache)
        {
            return await next();
        }
        TResponse response = await next();

        if (request.CacheGroupKey != null)
        {
            byte[]? cachedGroup = await _distributedCache.GetAsync(request.CacheGroupKey, cancellationToken);
            if (cachedGroup != null)
            {
                HashSet<string> keysInGroup = JsonSerializer.Deserialize<HashSet<string>>(Encoding.Default.GetString(cachedGroup))!;
                foreach (string key in keysInGroup)
                {
                    await _distributedCache.RemoveAsync(key, cancellationToken);
                }

                await _distributedCache.RemoveAsync(request.CacheGroupKey, cancellationToken);
                await _distributedCache.RemoveAsync(key: $"{request.CacheGroupKey}SlidingExpiration", cancellationToken);
            }
        }

        if (request.CacheKey != null)
        {
            await _distributedCache.RemoveAsync(request.CacheKey, cancellationToken);
        }

        return response;
    }
}

/*
Gelen bir isteği işlerken ilgili cache verilerini temizlemek için bir pipeline davranışı (behavior) sağlar.
Cache Bypass Kontrolü: 
    Eğer BypassCache true ise, önbellek silme işlemi yapılmadan işleyiciye geçilir.
Cache Grubunun Temizlenmesi: 
    İstek bir cache grubuna bağlı ise, gruptaki tüm cache anahtarları silinir ve grup bilgileri de temizlenir.
    Bu, toplu cache silme işlemi yapmayı kolaylaştırır.
Bireysel Cache Anahtarının Silinmesi: 
    Eğer istek bir CacheKey ile ilişkilendirilmişse, sadece bu anahtar silinir.
Bu yapı, veri güncelleme veya silme gibi işlemler sonrasında verilerin tutarlılığını sağlamak için önbelleğin temizlenmesini otomatik hale getirir.
Özellikle gruplama ile cache anahtarlarını toplu bir şekilde temizleyebilmek, büyük ve karmaşık sistemlerde cache yönetimini oldukça kolaylaştırır.
*/
