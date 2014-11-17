/*
 * TinyReflectiveToolkit
 * Copyright (C) 2014  Theodoros Chatzigiannakis
 * 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

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
        internal const BindingFlags GetMethodFlags = BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public |
                                                      BindingFlags.NonPublic;

        /// <summary>
        /// Returns all methods (including generic methods) that satisfy the specified constraints.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="parameters"></param>
        /// <param name="allowSubstitution"></param>
        /// <returns></returns>
        public static IEnumerable<MethodInfo> GetGenericMethods(this Type type, string name, ParameterInfo[] parameters, bool allowSubstitution)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (name == null) throw new ArgumentNullException("name");
            if (parameters == null) throw new ArgumentNullException("parameters");

            var allMethods = type.GetMethods(GetMethodFlags);
            var overloads = allMethods
                .Where(x => x.Name == name)
                .Where(x => x.GetParameters().Length == parameters.Length)
                .ToList();
            var matchingOverloads = overloads.Where(x => x.GetParameters()
                .SequenceEqual(parameters, (p1, p2) =>
                        ParamInfoPredicate.Invoke(p1.ParameterType, p2.ParameterType, allowSubstitution)))
                .ToList();
            return matchingOverloads;
        }

        /// <summary>
        /// Returns all methods (including generic methods) that satisfy the specified constraints.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static IEnumerable<MethodInfo> GetGenericMethods(this Type type, string name, ParameterInfo[] parameters)
        {
            return type.GetGenericMethods(name, parameters, false);
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
            return type.GetGenericMethods(name, parameters, false).SingleOrDefault();
        }

        /// <summary>
        /// Returns a method (including generic methods) that satisfies the specified constraints or null of none is found.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="parameters"></param>
        /// <param name="allowSubstitution"></param>
        /// <returns></returns>
        public static MethodInfo GetGenericMethod(this Type type, string name, ParameterInfo[] parameters, bool allowSubstitution)
        {
            return type.GetGenericMethods(name, parameters, allowSubstitution).SingleOrDefault();
        }

        internal static Func<Type, Type, bool, bool> ParamInfoPredicate = (t1, t2, sub) =>
        {
            if (t1.IsGenericParameter != t2.IsGenericParameter) return false;
            if (!t1.IsGenericParameter)
            {
                if (sub) return t1.IsAssignableFrom(t2);
                return t1 == t2;
            }
            return t1.GetGenericParameterConstraints()
                .SequenceEqual(t2.GetGenericParameterConstraints(), (x, y) => ParamInfoPredicate.Invoke(x, y, sub));
        };
    }
}
