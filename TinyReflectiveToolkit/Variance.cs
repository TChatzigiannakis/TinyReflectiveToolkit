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
