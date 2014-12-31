using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using EnumerableExtensions;
using System.Runtime.CompilerServices;
using EnumerableExtensions;

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
            return method.IsStatic && method.DeclaringType.IsSealed && method.HasAttribute<ExtensionAttribute>();
        }

        /// <summary>
        /// Gets the System.Type that this extension method can be called on.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public static Type TypeOfExtensionMethod(this MethodInfo method)
        {
            if (!method.IsExtensionMethod()) throw new ArgumentException(string.Format("The method {0} is not an extension method. This operation can only be performed on extension methods.", method.Name));
            return method.GetParameters().First().ParameterType;
        }

        /// <summary>
        /// Gets the extension methods available for this System.Type, within the provided assemblies.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        public static IEnumerable<MethodInfo> GetExtensionMethods(this Type type, IEnumerable<Assembly> assemblies)
        {
            return assemblies
                .Select(x => x.GetTypes())
                .Flatten()
                .Where(x => x.IsSealed)
                .Select(x => x.GetMethods())
                .Flatten()
                .Where(IsExtensionMethod);
        }

        /// <summary>
        /// Gets the extension methods available for this System.Type, from all loaded assemblies.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<MethodInfo> GetExtensionMethods(this Type type)
        {
            return type.GetExtensionMethods(AppDomain.CurrentDomain.GetAssemblies());
        }
    }
}
