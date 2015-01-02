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
using System.Reflection;
using System.Linq;

namespace TinyReflectiveToolkit
{
    /// <summary>
    /// Extension methods for the System.Reflection.Assembly class.
    /// </summary>
    public static class AssemblyExtensions
    {
        /// <summary>
        /// Gets all types in the assembly that can be loaded without errors.
        /// </summary>
        /// <returns>The loadable types.</returns>
        /// <param name="assembly">Assembly.</param>
        public static IEnumerable<Type> GetLoadableTypes(this Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");

            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.Where (t => t != null);
            }
        }
    }
}

