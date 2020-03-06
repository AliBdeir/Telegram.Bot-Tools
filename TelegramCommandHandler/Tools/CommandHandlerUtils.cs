using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace CommandHandler.Tools
{
    public static class CommandHandlerUtils
    {

        public static bool IsSameOrSubclass(Type potentialBase, Type potentialDescendant)
        {
            return potentialDescendant.IsSubclassOf(potentialBase)
                   || potentialDescendant == potentialBase;
        }

        public static bool IsAsync(this MethodInfo m)
        {
            return m?
                .GetCustomAttribute<AsyncStateMachineAttribute>()?
                .StateMachineType?
                .GetTypeInfo()
                .GetCustomAttribute<CompilerGeneratedAttribute>()
                != null;
        }


    }
}
