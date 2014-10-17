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
        /// Returns a generic method of a given type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<MethodInfo> GetGenericMethod(this Type type)
        {
            if (type == null) throw new ArgumentNullException("type");

            throw new NotImplementedException();
        }
    }
}
