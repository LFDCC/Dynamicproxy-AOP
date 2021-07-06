
using System;

namespace DynamicProxy_AOP_Autofac.Common
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CacheActionAttribute : Attribute
    {
        public int Seconds { get; set; } = 60;
    }
}
