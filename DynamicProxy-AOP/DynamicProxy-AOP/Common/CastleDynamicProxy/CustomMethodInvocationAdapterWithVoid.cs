using Castle.DynamicProxy;

using System;
using System.Threading.Tasks;

namespace DynamicProxy_AOP.Common.CastleDynamicProxy
{
    /// <summary>
    /// 没有返回值
    /// </summary>
    public class CustomMethodInvocationAdapterWithVoid : CustomMethodInvocationAdapterBase, ICustomMethodInvocation
    {
        protected IInvocationProceedInfo ProceedInfo { get; }
        protected Func<IInvocation, IInvocationProceedInfo, Task> Proceed { get; }

        public CustomMethodInvocationAdapterWithVoid(IInvocation invocation, IInvocationProceedInfo proceedInfo,
            Func<IInvocation, IInvocationProceedInfo, Task> proceed)
            : base(invocation)
        {
            ProceedInfo = proceedInfo;
            Proceed = proceed;
        }

        public override async Task ProceedAsync()
        {
            await Proceed(Invocation, ProceedInfo);
        }
    }
}
