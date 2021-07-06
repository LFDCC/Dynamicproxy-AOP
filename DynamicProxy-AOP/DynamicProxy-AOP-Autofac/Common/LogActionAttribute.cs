using System;

namespace DynamicProxy_AOP_Autofac.Common
{
    [AttributeUsage(AttributeTargets.Method)]
    public class LogActionAttribute : Attribute
    {
    }
}
