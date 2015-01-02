/*
 * TinyReflectiveToolkit
 * Copyright (C) 2014-2015  Theodoros Chatzigiannakis
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
