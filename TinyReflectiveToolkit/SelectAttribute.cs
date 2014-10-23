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

namespace TinyReflectiveToolkit
{
    public static partial class TypeExtensions
    {
        /// <summary>
        /// Returns all instances of the selected attribute in the given element.
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="info"></param>
        /// <returns></returns>
        public static IEnumerable<TAttribute> SelectAttribute<TAttribute>(this MemberInfo info)
            where TAttribute : Attribute
        {
            return info.GetCustomAttributes(typeof(TAttribute), false).Cast<TAttribute>();
        }
    }
}
