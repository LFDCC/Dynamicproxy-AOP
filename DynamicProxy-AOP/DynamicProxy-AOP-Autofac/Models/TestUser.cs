
using DynamicProxy_AOP_Autofac.Common;

using System;
using System.Threading.Tasks;

namespace DynamicProxy_AOP_Autofac.Models
{
    public class TestUser : ITestUser
    {

        [LogAction]
        public void Run(string arg)
        {
            Console.WriteLine($"=====这是Run 参数:{arg}====");
        }

        public string Run1(string arg)
        {
            Console.WriteLine($"=====这是Run1 参数:{arg}====");
            return $"=====这是Run1 参数:{arg}====";
        }

        public async Task Run2(string arg)
        {
            Console.WriteLine($"=====这是Run2 参数:{arg}====");
            await Task.CompletedTask;
        }

        [CacheAction(Seconds = 10)]
        public async Task<string> Run3(string arg)
        {
            Console.WriteLine($"=====这是Run3 参数:{arg}====");
            return await Task.FromResult($"=====这是Run3 参数:{arg},时间：{DateTime.Now}====");
        }
    }
}
