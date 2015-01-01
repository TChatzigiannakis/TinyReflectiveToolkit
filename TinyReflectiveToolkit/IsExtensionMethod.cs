using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using EnumerableExtensions;
using System.Runtime.CompilerServices;
using System.IO;

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
            return IsExtensionMethod(method, null);
        }

        /// <summary>
        /// Checks whether a method can be called as an extension method.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsExtensionMethod(this MethodInfo method, Type type)
        {
            try
            {
                if(!method.IsStatic) return false;
                if(!method.DeclaringType.IsSealed) return false;
                if(!method.HasAttribute<ExtensionAttribute>()) return false;
                if(type == null) return true;
                if(method.GetParameters() == null) return false;
                if(!method.GetParameters().Any()) return false;
                if(method.GetParameters().First().ParameterType == type) return true;
                return false;
            } catch(ReflectionTypeLoadException) {
                return false;
            } catch(FileNotFoundException ex) {
                return false;
            }
        }

        /// <summary>
        /// Gets the System.Type that this extension method can be called on.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public static Type GetTypeOfExtensionMethod(this MethodInfo method)
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
                .Select (x => x.GetLoadableTypes ())
                .Flatten ()
                .Where (x => x.IsSealed)
                .Select (x => x.GetMethods ())
                .Flatten ()
                .Where (x => IsExtensionMethod (x, type));
        }

        /// <summary>
        /// Gets the extension methods available for this System.Type, from all loaded assemblies.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<MethodInfo> GetExtensionMethods(this Type type)
        {
            return type.GetExtensionMethods (AppDomain.CurrentDomain.GetAssemblies ());
        }
    }
}
