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
        internal static IEnumerable<TMemberInfo> WithAttributeImpl<TAttribute, TMemberInfo>(this IEnumerable<TMemberInfo> sequence, bool inherited)
            where TAttribute : Attribute
            where TMemberInfo : MemberInfo
        {
            return sequence.Where(x => HasAttribute<TAttribute, TMemberInfo>(x, inherited));
        }
        internal static IEnumerable<TMemberInfo> WithoutAttributeImpl<TAttribute, TMemberInfo>(this IEnumerable<TMemberInfo> sequence, bool inherited)
            where TAttribute : Attribute
            where TMemberInfo : MemberInfo
        {
            return sequence.Except(x => HasAttribute<TAttribute, TMemberInfo>(x, inherited));
        }

        /// <summary>
        /// Returns all elements that are decorated with a specified attribute.
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <typeparam name="TMemberInfo"></typeparam>
        /// <param name="sequence"></param>
        /// <returns></returns>
        public static IEnumerable<TMemberInfo> WithAttribute<TAttribute, TMemberInfo>(this IEnumerable<TMemberInfo> sequence)
            where TAttribute : Attribute
            where TMemberInfo : MemberInfo
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            return sequence.WithAttributeImpl<TAttribute, TMemberInfo>(false);
        }

        /// <summary>
        /// Returns all elements that are decorated with a specified attribute and that attribute satisfies a specified condition.
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <typeparam name="TMemberInfo"></typeparam>
        /// <param name="sequence"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IEnumerable<TMemberInfo> WithAttribute<TAttribute, TMemberInfo>(this IEnumerable<TMemberInfo> sequence, 
            Func<TAttribute, bool> predicate)
            where TAttribute : Attribute
            where TMemberInfo : MemberInfo
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (predicate == null) throw new ArgumentNullException("predicate");

            return sequence.WithAttribute<TAttribute, TMemberInfo>().Where(x => x.SelectAttribute<TAttribute>().Any(predicate));
        }

        /// <summary>
        /// Returns all elements that are not decorated with a specified attribute.
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <typeparam name="TMemberInfo"></typeparam>
        /// <param name="sequence"></param>
        /// <returns></returns>
        public static IEnumerable<TMemberInfo> WithoutAttribute<TAttribute, TMemberInfo>(
            this IEnumerable<TMemberInfo> sequence)
            where TAttribute : Attribute
            where TMemberInfo : MemberInfo
        {
            if (sequence == null) throw new ArgumentNullException("sequence");

            return sequence.WithoutAttributeImpl<TAttribute, TMemberInfo>(false);
        }

        /// <summary>
        /// Returns all types that are decorated with a specified attribute.
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="sequence"></param>
        /// <returns></returns>
        public static IEnumerable<Type> WithAttribute<TAttribute>(this IEnumerable<Type> sequence)
            where TAttribute : Attribute
        {
            return sequence.WithAttribute<TAttribute, Type>();
        }

        /// <summary>
        /// Returns all types that are decorated with a specified attribute and that attribute satisfies a specified condition.
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="sequence"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IEnumerable<Type> WithAttribute<TAttribute>(this IEnumerable<Type> sequence,
            Func<TAttribute, bool> predicate)
            where TAttribute : Attribute
        {
            return sequence.WithAttribute<TAttribute, Type>(predicate);
        }

        /// <summary>
        /// Returns all methods that are decorated with a specified attribute.
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="sequence"></param>
        /// <returns></returns>
        public static IEnumerable<MethodInfo> WithAttribute<TAttribute>(this IEnumerable<MethodInfo> sequence)
            where TAttribute : Attribute
        {
            return sequence.WithAttribute<TAttribute, MethodInfo>();
        }

        /// <summary>
        /// Returns all methods that are decorated with a specified attribute and that attribute satisfies a specified condition.
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="sequence"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IEnumerable<MethodInfo> WithAttribute<TAttribute>(this IEnumerable<MethodInfo> sequence,
            Func<TAttribute, bool> predicate)
            where TAttribute : Attribute
        {
            return sequence.WithAttribute<TAttribute, MethodInfo>(predicate);
        }

        /// <summary>
        /// Returns all properties that are decorated with a specified attribute.
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="sequence"></param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> WithAttribute<TAttribute>(this IEnumerable<PropertyInfo> sequence)
            where TAttribute : Attribute
        {
            return sequence.WithAttribute<TAttribute, PropertyInfo>();
        }

        /// <summary>
        /// Returns all properties that are decorated with a specified attribute and that attribute satisfies a specified condition.
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="sequence"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> WithAttribute<TAttribute>(this IEnumerable<PropertyInfo> sequence, Func<TAttribute, bool> predicate)
            where TAttribute : Attribute
        {
            return sequence.WithAttribute<TAttribute, PropertyInfo>(predicate);
        }

        /// <summary>
        /// Returns all members that are decorated with a specified attribute.
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="sequence"></param>
        /// <returns></returns>
        public static IEnumerable<MemberInfo> WithAttribute<TAttribute>(this IEnumerable<MemberInfo> sequence)
            where TAttribute : Attribute
        {
            return sequence.WithAttribute<TAttribute, MemberInfo>();
        }

        /// <summary>
        /// Returns all members that are decorated with a specified attribute and that attribute satisfies a specified condition.
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="sequence"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IEnumerable<MemberInfo> WithAttribute<TAttribute>(this IEnumerable<MemberInfo> sequence, Func<TAttribute, bool> predicate)
            where TAttribute : Attribute
        {
            return sequence.WithAttribute<TAttribute, MemberInfo>(predicate);
        }

    }
}
