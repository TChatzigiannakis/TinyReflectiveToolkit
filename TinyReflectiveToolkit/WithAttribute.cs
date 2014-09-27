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
using System.Text;

namespace TinyReflectiveToolkit
{
    public static partial class TypeExtensions
    {
        /// <summary>
        /// Returns all types that are decorated with a specified attribute.
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="sequence"></param>
        /// <returns></returns>
        public static IEnumerable<Type> WithAttribute<TAttribute>(this IEnumerable<Type> sequence)
            where TAttribute : Attribute
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            return sequence.WithAttribute<TAttribute>(false);
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
            if (sequence == null) throw new ArgumentNullException("sequence");
            if (predicate == null) throw new ArgumentNullException("predicate");

            return sequence.WithAttribute<TAttribute>().Where(x => x.SelectAttribute<TAttribute>().Any(predicate));
        }

        private static IEnumerable<Type> WithAttribute<TAttribute>(this IEnumerable<Type> sequence, bool inherited)
            where TAttribute : Attribute
        {
            return sequence.Where(x => x.GetCustomAttributes(typeof (TAttribute), inherited).Any());
        }

        /// <summary>
        /// Returns all types that are decorated with a specified attribute, including those that have inherited the attribute.
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="sequence"></param>
        /// <param name="inherited"></param>
        /// <returns></returns>
        public static IEnumerable<Type> WithInheritedAttribute<TAttribute>(this IEnumerable<Type> sequence)
            where TAttribute : Attribute
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            return sequence.WithAttribute<TAttribute>(true);
        }
    }
}
