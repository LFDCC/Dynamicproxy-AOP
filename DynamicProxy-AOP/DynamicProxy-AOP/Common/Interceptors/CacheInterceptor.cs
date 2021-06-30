using DynamicProxy_AOP.Common.CastleDynamicProxy;
using DynamicProxy_AOP.Models;

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

using System;
using System.Threading.Tasks;

namespace DynamicProxy_AOP.Common.Interceptors
{
    public class CacheInterceptor : CustomInterceptor
    {
        IMemoryCache _cache;
        public CacheInterceptor(IMemoryCache cache)
        {
            this._cache = cache;
        }
        public override async Task InterceptAsync(ICustomMethodInvocation invocation)
        {
            if (invocation.Method != null && invocation.Method.IsDefined(typeof(CacheActionAttribute), true))
            {
                var cache = (CacheActionAttribute)invocation.Method.GetCustomAttributes(typeof(CacheActionAttribute), true)[0];

                var result = _cache.Get(invocation.Method);
                if (result != null)
                {
                    invocation.ReturnValue = result;
                    return;
                }
                await invocation.ProceedAsync();
                if (invocation.ReturnValue != null)
                {
                    _cache.Set(invocation.Method, invocation.ReturnValue, new MemoryCacheEntryOptions
                    {
                        AbsoluteExpiration = DateTime.Now.AddSeconds(cache.Seconds)
                    });
                }
            }
        }
    }
}
