/*
 * TinyReflectiveToolkit
 * Copyright (C) 2014  Theodoros Chatzigiannakis
 * 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace TinyReflectiveToolkit.Contracts
{
    /// <summary>
    /// This class exposes various special operations of the runtime as static methods.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class SpecialOperations
    {
        internal static MethodInfo IdentityMarkerMethodInfo
        {
            get { return typeof (SpecialOperations).GetMethods().Single(m => m.Name == "IdentityMarker"); }
        }

        internal static MethodInfo GetSpecialConversion(Type source, Type target)
        {
            var matches = typeof (SpecialOperations).GetMethods()
                .WithAttribute<SpecialConversionAttribute>()
                .Where(x => x.GetParameters().Count() == 1)
                .Where(x => x.GetParameters().First().ParameterType == source)
                .Where(x => x.ReturnType == target)
                .ToList();
            return matches.SingleOrDefault();
        }
        
        /// <summary>
        /// Empty method, marking the need for a runtime-generated (non-generic) identity function.
        /// </summary>
        public static void IdentityMarker()
        {            
        }

        /// <summary>
        /// Casts float to int.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [SpecialConversion]
        public static int FloatToInt(float input)
        {
            return (int)input;
        }

        /// <summary>
        /// Casts double to int.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [SpecialConversion]
        public static int DoubleToInt(double input)
        {
            return (int) input;
        }

        /// <summary>
        /// Casts byte to int.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [SpecialConversion]
        public static int ByteToInt(byte input)
        {
            return input;
        }
    }
}
