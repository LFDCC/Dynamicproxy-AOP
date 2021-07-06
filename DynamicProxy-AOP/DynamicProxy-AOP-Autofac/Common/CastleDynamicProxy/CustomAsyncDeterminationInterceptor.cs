using Castle.DynamicProxy;

namespace DynamicProxy_AOP_Autofac.Common.CastleDynamicProxy
{
    public class CustomAsyncDeterminationInterceptor<TInterceptor> : AsyncDeterminationInterceptor
        where TInterceptor : ICustomInterceptor
    {
        public CustomAsyncDeterminationInterceptor(TInterceptor customInterceptor)
            : base(new CustomInterceptorAdapter<TInterceptor>(customInterceptor))
        {

        }
    }
}