using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicProxy_AOP.Common
{
    [AttributeUsage(AttributeTargets.Method)]
    public class LogActionAttribute : Attribute
    {
    }
}
