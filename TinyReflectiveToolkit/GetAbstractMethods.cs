using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TinyReflectiveToolkit
{
    public static partial class TypeExtensions
    {
        /// <summary>
        /// Returns all abstract methods of a given type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<MethodInfo> GetAbstractMethods(this Type type)
        {
            if (type == null) throw new ArgumentNullException("type");

            return type.GetMethods().Where(x => x.IsAbstract);
        }
    }
}
