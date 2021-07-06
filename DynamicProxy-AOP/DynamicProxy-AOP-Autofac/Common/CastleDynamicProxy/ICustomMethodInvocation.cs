using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace DynamicProxy_AOP_Autofac.Common.CastleDynamicProxy
{
    public interface ICustomMethodInvocation
    {
        object[] Arguments { get; }

        IReadOnlyDictionary<string, object> ArgumentsDictionary { get; }

        Type[] GenericArguments { get; }

        object TargetObject { get; }

        MethodInfo Method { get; }

        object ReturnValue { get; set; }

		Task ProceedAsync();
    }
}