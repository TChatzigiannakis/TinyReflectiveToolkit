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
using System.Text;

namespace TinyReflectiveToolkit
{
    public static partial class TypeExtensions
    {
        /// <summary>
        /// Checks whether the given type is a delegate.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsDelegate(this Type type)
        {
            return typeof (Delegate).IsAssignableFrom(type);
        }
    }
}
