using System.Threading.Tasks;

namespace DynamicProxy_AOP_Autofac.Common.CastleDynamicProxy
{
    /// <summary>
    /// 抽象 拦截器
    /// </summary>
	public abstract class CustomInterceptor : ICustomInterceptor
    {
        public abstract Task InterceptAsync(ICustomMethodInvocation invocation);
    }
}