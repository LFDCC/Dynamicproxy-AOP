
using DynamicProxy_AOP.Common.CastleDynamicProxy;
using DynamicProxy_AOP.Models;

using System;
using System.Threading.Tasks;
using System.Text.Json;

namespace DynamicProxy_AOP.Common.Interceptors
{
    public class LogInterceptor : CustomInterceptor
    {
        public override async Task InterceptAsync(ICustomMethodInvocation invocation)
        {
            try
            {
                if (invocation.Method != null && invocation.Method.IsDefined(typeof(LogActionAttribute), true))
                {
                    Console.WriteLine("日志记录开始==============");
                    Console.WriteLine($"===========这是{invocation.Method.Name}方法的开始===========");

                    Console.WriteLine($"===========这是{invocation.Method.Name}方法的请求参数：{JsonSerializer.Serialize(invocation.ArgumentsDictionary)}===========");

                    await invocation.ProceedAsync();

                    Console.WriteLine($"===========这是{invocation.Method.Name}方法的返回值：{invocation.ReturnValue}===========");

                    Console.WriteLine($"===========这是{invocation.Method.Name}方法的结束===========");
                    Console.WriteLine("日志记录结束==============");
                }
                else
                {
                    await invocation.ProceedAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("这里是异常信息：{0}", ex);
            }

        }
    }

}
