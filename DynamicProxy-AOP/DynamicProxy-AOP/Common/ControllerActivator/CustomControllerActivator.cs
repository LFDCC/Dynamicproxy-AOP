using Castle.DynamicProxy;

using DynamicProxy_AOP.Common.CastleDynamicProxy;
using DynamicProxy_AOP.Controllers;
using DynamicProxy_AOP.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;

namespace DynamicProxy_AOP.Common.ControllerActivator
{
    public class CustomControllerActivator : IControllerActivator
    {
        public object Create(ControllerContext actionContext)
        {
            if (actionContext == null)
            {
                throw new ArgumentNullException(nameof(actionContext));
            }

            Type serviceType = actionContext.ActionDescriptor.ControllerTypeInfo.AsType();

            var target = actionContext.HttpContext.RequestServices.GetRequiredService(serviceType);

            var customInterceptors = actionContext.HttpContext.RequestServices.GetRequiredService<IEnumerable<ICustomInterceptor>>();

            var interceptors = GetInterceptors(customInterceptors);


            var consts = target.GetType().GetConstructors()[0];

            object[] obj = new object[consts.GetParameters().Length];
            for (int i = 0; i < consts.GetParameters().Length; i++)
            {
                var pm = consts.GetParameters()[i];

                var vl = actionContext.HttpContext.RequestServices.GetRequiredService(pm.ParameterType);

                obj[i] = vl;
            }
            //CreateClassProxyWithTarget 使用父类实现代理拦截 需要将方法标记虚方法 virtual
            var proxy = new ProxyGenerator().CreateClassProxyWithTarget(serviceType, target, obj, interceptors.ToInterceptors());

            return proxy;
        }
        public virtual void Release(ControllerContext context, object controller)
        {
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
