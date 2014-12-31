using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using EnumerableExtensions;
using System.Runtime.CompilerServices;

namespace TinyReflectiveToolkit
{
    public static partial class TypeExtensions
    {
        /// <summary>
        /// Checks whether a method can be called as an extension method.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public static bool IsExtensionMethod(this MethodInfo method)
        {
            return method.ToUnarySequence().WithAttribute<ExtensionAttribute>().Any();
        }

    }
}
