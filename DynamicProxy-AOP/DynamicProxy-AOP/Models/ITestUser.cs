using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicProxy_AOP.Models
{
    public interface ITestUser
    {
        void Run(string arg);

        string Run1(string arg);

        Task Run2(string arg);

        Task<string> Run3(string arg);

    }
}
