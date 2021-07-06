using System.Threading.Tasks;

namespace DynamicProxy_AOP_Autofac.Common.CastleDynamicProxy
{
    public interface ICustomInterceptor
    {
        Task InterceptAsync(ICustomMethodInvocation invocation);
	}
}
