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
using System.Reflection.Emit;
using System.Text;

namespace TinyReflectiveToolkit.IL
{
    internal static class ILHelper
    {
        /// <summary>
        /// Generates a method stub that returns the value of a field.
        /// </summary>
        /// <param name="containingType"></param>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="field"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static MethodBuilder GenerateMethodStubThatReturnsField(TypeBuilder containingType, string name, MethodAttributes attributes, FieldInfo field, Type returnType, Type[] parameters)
        {
            var method = containingType.DefineMethod(name, attributes, returnType ?? field.FieldType, parameters);
            var gen = method.GetILGenerator();
            gen.Emit(OpCodes.Ldarg_0);
            gen.Emit(OpCodes.Ldfld, field);
            gen.Emit(OpCodes.Ret);
            return method;
        }

        public static MethodBuilder GenerateCallStubTo(MethodInfo callee, TypeBuilder containingType)
        {
            return null;
        }
    }
}
