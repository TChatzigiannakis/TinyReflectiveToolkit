using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using EnumerableExtensions;

namespace TinyReflectiveToolkit
{
    public static partial class TypeExtensions
    {        
        /// <summary>
        /// Returns all methods (including generic methods) that satisfy the specified constraints.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static IEnumerable<MethodInfo> GetGenericMethods(this Type type, string name, ParameterInfo[] parameters)
        {
            if (type == null) throw new ArgumentNullException("type");

            var allMethods = type.GetMethods(BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var overloads = allMethods
                .Where(x => x.Name == name)
                .Where(x => x.GetParameters().Length == parameters.Length)
                .ToList();
            var matchingOverloads = overloads.Where(x => x.GetParameters()
                .SequenceEqual(parameters, (p1, p2) =>
                        ParamInfoPredicate.Invoke(p1.ParameterType, p2.ParameterType)))
                .ToList();
            return matchingOverloads;
        }

        /// <summary>
        /// Returns a method (including generic methods) that satisfies the specified constraints or null of none is found.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static MethodInfo GetGenericMethod(this Type type, string name, ParameterInfo[] parameters)
        {
            return type.GetGenericMethods(name, parameters).SingleOrDefault();
        }

        internal static Func<Type, Type, bool> ParamInfoPredicate = (t1, t2) =>
        {
            if (t1.IsGenericParameter != t2.IsGenericParameter) return false;
            if (!t1.IsGenericParameter) return t1 == t2;
            return t1.GetGenericParameterConstraints()
                .SequenceEqual(t2.GetGenericParameterConstraints(), ParamInfoPredicate);
        };
    }
}
