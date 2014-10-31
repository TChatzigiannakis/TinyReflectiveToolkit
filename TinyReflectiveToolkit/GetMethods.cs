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
using EnumerableExtensions;

namespace TinyReflectiveToolkit
{
    public static partial class TypeExtensions
    {
        /// <summary>
        /// Returns all methods from a given sequence of types. 
        /// </summary>
        /// <param name="sequence"></param>
        /// <returns></returns>
        public static IEnumerable<MethodInfo> GetMethods(this IEnumerable<Type> sequence)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");

            return sequence.Select(x => x.GetMethods()).SelectMany(x => x);
        }

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
