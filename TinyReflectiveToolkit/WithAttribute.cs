/*
 *  Tiny Reflective Toolkit
    Copyright (C) 2014  Theodoros Chatzigiannakis

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>. 
 */

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

        private static IEnumerable<TMemberInfo> WithAttributeImpl<TAttribute, TMemberInfo>(this IEnumerable<TMemberInfo> sequence, bool inherited)
            where TAttribute : Attribute
            where TMemberInfo : MemberInfo
        {
            return sequence.Where(x => x.GetCustomAttributes(typeof (TAttribute), inherited).Any());
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
    }
}
