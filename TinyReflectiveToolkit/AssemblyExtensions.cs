using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace TinyReflectiveToolkit
{
    /// <summary>
    /// Extension methods for the System.Reflection.Assembly class.
    /// </summary>
    public static class AssemblyExtensions
    {
        /// <summary>
        /// Gets all types in the assembly that can be loaded without errors.
        /// </summary>
        /// <returns>The loadable types.</returns>
        /// <param name="assembly">Assembly.</param>
        public static IEnumerable<Type> GetLoadableTypes(this Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");

            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.Where (t => t != null);
            }
        }
    }
}

