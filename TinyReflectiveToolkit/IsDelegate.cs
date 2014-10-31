using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TinyReflectiveToolkit
{
    public static partial class TypeExtensions
    {
        /// <summary>
        /// Checks whether the given type is a delegate.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsDelegate(this Type type)
        {
            return typeof (Delegate).IsAssignableFrom(type);
        }
    }
}
