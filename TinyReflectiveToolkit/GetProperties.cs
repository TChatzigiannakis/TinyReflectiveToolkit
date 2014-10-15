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
        /// Returns all properties from a given sequence of types. 
        /// </summary>
        /// <param name="sequence"></param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> GetProperties(this IEnumerable<Type> sequence)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");

            return sequence.Select(x => x.GetProperties()).SelectMany(x => x);
        }
    }
}
