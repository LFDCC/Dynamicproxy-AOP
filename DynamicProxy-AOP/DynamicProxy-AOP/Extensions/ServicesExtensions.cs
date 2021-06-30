using Castle.DynamicProxy;

using DynamicProxy_AOP.Common.CastleDynamicProxy;
using DynamicProxy_AOP.Common.CustomControllerProxy;

using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
                var customInterceptors = serviceProvider.GetRequiredService<IEnumerable<ICustomInterceptor>>();
                var interceptors = GetInterceptors(customInterceptors);
                var proxy = proxyGenerator.CreateInterfaceProxyWithTarget<TService>(target, interceptors.ToInterceptors());
                return proxy;
            }, serviceLifetime));

            return services;
        }

        private static IEnumerable<IAsyncInterceptor> GetInterceptors(IEnumerable<ICustomInterceptor> customInterceptors)
        {
            foreach (var customInterceptor in customInterceptors)
            {
                var interceptor = new CustomInterceptorAdapter<ICustomInterceptor>(customInterceptor);

                yield return interceptor;
            }
        }
    }
}
