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
        /// Returns all operator methods.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<MethodInfo> GetOperators(this Type type)
        {
            return type.GetMethods()
                .Where(x => x.IsStatic)
                .Where(x => x.Name.StartsWith("op_"));
        }

        /// <summary>
        /// Returns all explicit operator methods.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<MethodInfo> GetExplicitOperators(this Type type)
        {
            return type.GetOperators()
                .Where(x => x.Name == "op_Explicit");
        }

        /// <summary>
        /// Returns all explicit operator methods that return the specified type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="returnType"></param>
        /// <returns></returns>
        public static IEnumerable<MethodInfo> GetExplicitOperators(this Type type, Type returnType)
        {
            return type.GetExplicitOperators()
                .Where(x => x.ReturnType == returnType);
        }

        /// <summary>
        /// Returns all implicit operator methods.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<MethodInfo> GetImplicitOperators(this Type type)
        {
            return type.GetOperators()
                .Where(x => x.Name == "op_Implicit");
        }

        /// <summary>
        /// Returns all implicit operator methods that return the specified type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="returnType"></param>
        /// <returns></returns>
        public static IEnumerable<MethodInfo> GetImplicitOperators(this Type type, Type returnType)
        {
            return type.GetImplicitOperators()
                .Where(x => x.ReturnType == returnType);
        }
    }
}
