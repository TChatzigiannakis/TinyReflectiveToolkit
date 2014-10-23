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
        /// Checks whether an instance of the current type can be assigned to a variable of the target type, applying the specified variance.
        /// </summary>
        /// <param name="sourceType"></param>
        /// <param name="targetType"></param>
        /// <param name="variance"></param>
        /// <returns></returns>
        public static bool CanBeCastTo(this Type sourceType, Type targetType, Variance variance)
        {
            if (variance == Variance.Invariant)
                return sourceType == targetType;
            if (variance == Variance.Covariant)
                return targetType.IsAssignableFrom(sourceType);
            return sourceType.IsAssignableFrom(targetType);
        }
    }

    /// <summary>
    /// Specifies type check variance.
    /// </summary>
    public enum Variance
    {
        /// <summary>
        /// Specifies an invariant type check.
        /// </summary>
        Invariant, 
        /// <summary>
        /// Specifies a covariant (out) type check.
        /// </summary>
        Covariant, 
        /// <summary>
        /// Specifies a contravariant (in) type check.
        /// </summary>
        Contravariant
    }
}
