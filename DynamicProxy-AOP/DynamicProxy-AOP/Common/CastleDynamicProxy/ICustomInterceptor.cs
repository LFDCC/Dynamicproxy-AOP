using System.Threading.Tasks;

namespace DynamicProxy_AOP.Common.CastleDynamicProxy
{
    public interface ICustomInterceptor
    {
        Task InterceptAsync(ICustomMethodInvocation invocation);
	}
}
