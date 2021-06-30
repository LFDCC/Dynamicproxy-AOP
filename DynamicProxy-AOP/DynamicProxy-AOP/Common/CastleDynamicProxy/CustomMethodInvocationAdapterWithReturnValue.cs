using Castle.DynamicProxy;

using System;
using System.Threading.Tasks;

namespace DynamicProxy_AOP.Common.CastleDynamicProxy
{
    /// <summary>
    /// 有返回值
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public class CustomMethodInvocationAdapterWithReturnValue<TResult> : CustomMethodInvocationAdapterBase, ICustomMethodInvocation
    {
        protected IInvocationProceedInfo ProceedInfo { get; }
        protected Func<IInvocation, IInvocationProceedInfo, Task<TResult>> Proceed { get; }

        public CustomMethodInvocationAdapterWithReturnValue(IInvocation invocation,
            IInvocationProceedInfo proceedInfo,
            Func<IInvocation, IInvocationProceedInfo, Task<TResult>> proceed)
            : base(invocation)
        {
            ProceedInfo = proceedInfo;
            Proceed = proceed;
        }

        public override async Task ProceedAsync()
        {
            ReturnValue = await Proceed(Invocation, ProceedInfo);
        }
    }
}
