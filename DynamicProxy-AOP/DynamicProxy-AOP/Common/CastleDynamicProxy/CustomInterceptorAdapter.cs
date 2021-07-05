using Castle.DynamicProxy;

using System;
using System.Threading.Tasks;

namespace DynamicProxy_AOP.Common.CastleDynamicProxy
{
    /// <summary>
    /// 拦截器/适配器
    /// </summary>
    /// <typeparam name="TInterceptor">泛型类型（用于回调的拦截器）</typeparam>
    public class CustomInterceptorAdapter<TInterceptor> : AsyncInterceptorBase
        where TInterceptor : ICustomInterceptor
    {
        private readonly TInterceptor _customInterceptor;

        public CustomInterceptorAdapter(TInterceptor customInterceptor)
        {
            _customInterceptor = customInterceptor;
        }

        protected override async Task InterceptAsync(IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task> proceed)
        {
            await _customInterceptor.InterceptAsync(
                new CustomMethodInvocationAdapterWithVoid(invocation, proceedInfo, proceed)
            );
        }

        protected override async Task<TResult> InterceptAsync<TResult>(IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task<TResult>> proceed)
        {
            var adapter = new CustomMethodInvocationAdapterWithReturnValue<TResult>(invocation, proceedInfo, proceed);

            await _customInterceptor.InterceptAsync(
                adapter
            );

            return (TResult)adapter.ReturnValue;
        }
    }
}