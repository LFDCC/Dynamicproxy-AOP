using Castle.DynamicProxy;

using DynamicProxy_AOP.Common.CastleDynamicProxy;

using Microsoft.Extensions.DependencyInjection;

using System.Collections.Generic;

namespace DynamicProxy_AOP.Extensions
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddProxyService<TService, TImplementation>(this IServiceCollection services, ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
          where TImplementation : class, TService
           where TService : class
        {

            services.Add(new ServiceDescriptor(typeof(TImplementation), typeof(TImplementation), serviceLifetime));

            services.Add(new ServiceDescriptor(typeof(TService), serviceProvider =>
            {
                var target = serviceProvider.GetRequiredService<TImplementation>();
                var proxyGenerator = serviceProvider.GetRequiredService<ProxyGenerator>();
                var abpInterceptors = serviceProvider.GetRequiredService<IEnumerable<ICustomInterceptor>>();
                var interceptors = GetInterceptors(abpInterceptors);
                var proxy = proxyGenerator.CreateInterfaceProxyWithTarget<TService>(target, interceptors.ToInterceptors());
                return proxy;
            }, serviceLifetime));

            return services;
        }

        private static IEnumerable<IAsyncInterceptor> GetInterceptors(IEnumerable<ICustomInterceptor> abpInterceptors)
        {
            foreach (var abpInterceptor in abpInterceptors)
            {
                var interceptor = new CustomInterceptorAdapter<ICustomInterceptor>(abpInterceptor); //(IInterceptor)serviceProvider.GetRequiredService(typeof(AbpAsyncDeterminationInterceptor<>).MakeGenericType(typeof(IAbpInterceptor)));

                yield return interceptor;
            }
        }
    }
}
