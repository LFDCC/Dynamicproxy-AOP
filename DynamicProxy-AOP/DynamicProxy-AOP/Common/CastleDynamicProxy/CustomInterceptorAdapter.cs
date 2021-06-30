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
        private readonly TInterceptor _abpInterceptor;

        public CustomInterceptorAdapter(TInterceptor abpInterceptor)
        {
            _abpInterceptor = abpInterceptor;
        }

        protected override async Task InterceptAsync(IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task> proceed)
        {
            await _abpInterceptor.InterceptAsync(
                new CustomMethodInvocationAdapterWithVoid(invocation, proceedInfo, proceed)
            );
        }

        protected override async Task<TResult> InterceptAsync<TResult>(IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task<TResult>> proceed)
        {
            var adapter = new CustomMethodInvocationAdapterWithReturnValue<TResult>(invocation, proceedInfo, proceed);

            await _abpInterceptor.InterceptAsync(
                adapter
            );

            return (TResult)adapter.ReturnValue;
        }
    }
}