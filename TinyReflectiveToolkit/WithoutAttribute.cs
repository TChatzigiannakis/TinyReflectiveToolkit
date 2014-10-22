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
