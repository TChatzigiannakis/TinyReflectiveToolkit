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
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

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
