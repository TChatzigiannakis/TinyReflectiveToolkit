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
using System.Reflection;

namespace TinyReflectiveToolkit
{
    public static partial class TypeExtensions
    {
        /// <summary>
        /// Returns all types that are not decorated with a specified attribute.
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="sequence"></param>
        /// <returns></returns>
        public static IEnumerable<Type> WithoutAttribute<TAttribute>(this IEnumerable<Type> sequence)
            where TAttribute : Attribute
        {
            return sequence.WithoutAttribute<TAttribute, Type>();
        }

        /// <summary>
        /// Returns all methods that are not decorated with a specified attribute.
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="sequence"></param>
        /// <returns></returns>
        public static IEnumerable<MethodInfo> WithoutAttribute<TAttribute>(this IEnumerable<MethodInfo> sequence)
            where TAttribute : Attribute
        {
            return sequence.WithoutAttribute<TAttribute, MethodInfo>();
        }

        /// <summary>
        /// Retyrns all properties that are not decorated with a specified attribute.
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="sequence"></param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> WithoutAttribute<TAttribute>(this IEnumerable<PropertyInfo> sequence)
            where TAttribute : Attribute
        {
            return sequence.WithoutAttribute<TAttribute, PropertyInfo>();
        }
    }
}
