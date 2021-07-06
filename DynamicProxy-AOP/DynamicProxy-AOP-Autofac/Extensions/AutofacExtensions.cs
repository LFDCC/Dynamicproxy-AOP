using Autofac.Builder;
using Autofac.Extras.DynamicProxy;

using DynamicProxy_AOP_Autofac.Common.CastleDynamicProxy;

using System;

namespace DynamicProxy_AOP_Autofac.Extensions
{
    public static class AutofacExtensions
    {
        public static IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> AddInterceptors<TLimit, TActivatorData, TRegistrationStyle>(
                 this IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> registrationBuilder, Type[] interceptors)
             where TActivatorData : ReflectionActivatorData
        {
            foreach (var interceptor in interceptors)
            {
                registrationBuilder.InterceptedBy(
                    typeof(CustomAsyncDeterminationInterceptor<>).MakeGenericType(interceptor)
                );
            }
            return registrationBuilder;
        }

    }
}
