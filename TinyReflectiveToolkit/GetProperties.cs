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
        /// Returns all properties from a given sequence of types. 
        /// </summary>
        /// <param name="sequence"></param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> GetProperties(this IEnumerable<Type> sequence)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");

            return sequence.Select(x => x.GetProperties()).SelectMany(x => x);
        }
    }
}
