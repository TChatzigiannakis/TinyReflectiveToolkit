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
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using EnumerableExtensions;

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

        internal static MethodInfo GetSpecialOperator(Type operatorMarker, Type input1, Type input2, Type output, bool reverse)
        {
            var matches = typeof (SpecialOperations).GetMethods()
                .WithAttribute<SpecialOperatorAttribute>(x => x.Type == operatorMarker)
                .Where(x => x.GetParameters().Count() == 2)
                .Where(x => !reverse && x.GetParameters().First().ParameterType == input1
                    , x => reverse && x.GetParameters().First().ParameterType.IsAssignableFrom(input2)
                    )
                .Where(x => !reverse && x.GetParameters().Second().ParameterType.IsAssignableFrom(input2)
                    , x => reverse && x.GetParameters().Second().ParameterType == input1
                    )
                .Where(x => output == x.ReturnType)
                .ToList();
            return matches.FirstOrDefault();
        }
        
        /// <summary>
        /// Empty method, marking the need for a runtime-generated (non-generic) identity function.
        /// </summary>
        public static void IdentityMarker()
        {            
        }

        /// <summary>
        /// Special cast.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [SpecialConversion]
        public static int FloatToInt(float input)
        {
            return (int)input;
        }

        /// <summary>
        /// Special cast.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [SpecialConversion]
        public static int DoubleToInt(double input)
        {
            return (int) input;
        }

        /// <summary>
        /// Special cast.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [SpecialConversion]
        public static int ByteToInt(byte input)
        {
            return input;
        }

        /// <summary>
        /// Special operator.
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <returns></returns>
        [SpecialOperator(typeof(AdditionAttribute))]
        public static string Concat(string str1, string str2)
        {
            return string.Concat(str1, str2);
        }
        
        /// <summary>
        /// Special operator.
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <returns></returns>
        [SpecialOperator(typeof(AdditionAttribute))]
        public static string Concat(int str1, string str2)
        {
            return string.Concat(str1, str2);
        }

        /// <summary>
        /// Special operator.
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <returns></returns>
        [SpecialOperator(typeof(AdditionAttribute))]
        public static string Concat(string str1, int str2)
        {
            return string.Concat(str1, str2);
        }

        /// <summary>
        /// Special operator.
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <returns></returns>
        [SpecialOperator(typeof(AdditionAttribute))]
        public static string Concat(object str1, string str2)
        {
            return string.Concat(str1, str2);
        }

        /// <summary>
        /// Special operator.
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <returns></returns>
        [SpecialOperator(typeof(AdditionAttribute))]
        public static string Concat(string str1, object str2)
        {
            return string.Concat(str1, str2);
        }

        /// <summary>
        /// Special operator.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        [SpecialOperator(typeof(AdditionAttribute))]
        public static int Add(int a, int b)
        {
            return a + b;
        }

        /// <summary>
        /// Special operator.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        [SpecialOperator(typeof(AdditionAttribute))]
        public static object AddObject(int a, int b)
        {
            return a + b;
        }

        /// <summary>
        /// Special operator.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        [SpecialOperator(typeof(AdditionAttribute))]
        public static float Add(int a, float b)
        {
            return a + b;
        }

        /// <summary>
        /// Special operator.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        [SpecialOperator(typeof(AdditionAttribute))]
        public static float Add(float a, int b)
        {
            return a + b;
        }

        /// <summary>
        /// Special operator.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        [SpecialOperator(typeof(AdditionAttribute))]
        public static double Add(double a, int b)
        {
            return a + b;
        }

        /// <summary>
        /// Special operator.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        [SpecialOperator(typeof(AdditionAttribute))]
        public static double Add(int a, double b)
        {
            return a + b;
        }

        /// <summary>
        /// Special operator.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        [SpecialOperator(typeof(AdditionAttribute))]
        public static int Add(int a, byte b)
        {
            return a + b;
        }

        /// <summary>
        /// Special operator.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        [SpecialOperator(typeof(AdditionAttribute))]
        public static int Add(byte a, int b)
        {
            return a + b;
        }
    }

    internal static class OtherWhere
    {
        public static IEnumerable<T> Where<T>(this IEnumerable<T> sequence, Func<T, bool> predicate1,
            Func<T, bool> predicate2)
        {
            return sequence.Where(x => predicate1.Invoke(x) || predicate2.Invoke(x));
        } 
    }
}
