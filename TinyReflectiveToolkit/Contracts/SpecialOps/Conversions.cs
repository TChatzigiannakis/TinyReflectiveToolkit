/*
 * TinyReflectiveToolkit
 * Copyright (C) 2014  Theodoros Chatzigiannakis
 * 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System;

namespace TinyReflectiveToolkit.Contracts.SpecialOps
{
    public static partial class SpecialOperations
    {
        /// <summary>
        /// Special cast.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [SpecialConversion]
        [Obsolete(SpecialOperationsUsageErrorMessage, true)]
        public static double ToDouble(float input)
        {
            return input;
        }

        /// <summary>
        /// Special cast.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [SpecialConversion]
        [Obsolete(SpecialOperationsUsageErrorMessage, true)]
        public static double ToDouble(int input)
        {
            return input;
        }

        /// <summary>
        /// Special cast.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [SpecialConversion]
        [Obsolete(SpecialOperationsUsageErrorMessage, true)]
        public static double ToDouble(char input)
        {
            return input;
        }

        /// <summary>
        /// Special cast.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [SpecialConversion]
        [Obsolete(SpecialOperationsUsageErrorMessage, true)]
        public static double ToDouble(byte input)
        {
            return input;
        }

        /// <summary>
        /// Special cast.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [SpecialConversion]
        [Obsolete(SpecialOperationsUsageErrorMessage, true)]
        public static float ToFloat(double input)
        {
            return (float)input;
        }

        /// <summary>
        /// Special cast.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [SpecialConversion]
        [Obsolete(SpecialOperationsUsageErrorMessage, true)]
        public static float ToFloat(int input)
        {
            return input;
        }

        /// <summary>
        /// Special cast.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [SpecialConversion]
        [Obsolete(SpecialOperationsUsageErrorMessage, true)]
        public static float ToFloat(char input)
        {
            return input;
        }

        /// <summary>
        /// Special cast.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [SpecialConversion]
        [Obsolete(SpecialOperationsUsageErrorMessage, true)]
        public static float ToFloat(byte input)
        {
            return input;
        }

        /// <summary>
        /// Special cast.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [SpecialConversion]
        [Obsolete(SpecialOperationsUsageErrorMessage, true)]
        public static int ToInt(double input)
        {
            return (int)input;
        }

        /// <summary>
        /// Special cast.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [SpecialConversion]
        [Obsolete(SpecialOperationsUsageErrorMessage, true)]
        public static int ToInt(float input)
        {
            return (int)input;
        }
        
        /// <summary>
        /// Special cast.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [SpecialConversion]
        [Obsolete(SpecialOperationsUsageErrorMessage, true)]
        public static int ToInt(char input)
        {
            return input;
        }

        /// <summary>
        /// Special cast.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [SpecialConversion]
        [Obsolete(SpecialOperationsUsageErrorMessage, true)]
        public static int ToInt(byte input)
        {
            return input;
        }

        /// <summary>
        /// Special cast.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [SpecialConversion]
        [Obsolete(SpecialOperationsUsageErrorMessage, true)]
        public static char ToChar(double input)
        {
            return (char)input;
        }

        /// <summary>
        /// Special cast.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [SpecialConversion]
        [Obsolete(SpecialOperationsUsageErrorMessage, true)]
        public static char ToChar(float input)
        {
            return (char)input;
        }

        /// <summary>
        /// Special cast.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [SpecialConversion]
        [Obsolete(SpecialOperationsUsageErrorMessage, true)]
        public static char ToChar(int input)
        {
            return (char)input;
        }

        /// <summary>
        /// Special cast.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [SpecialConversion]
        [Obsolete(SpecialOperationsUsageErrorMessage, true)]
        public static char ToChar(byte input)
        {
            return (char)input;
        }

        /// <summary>
        /// Special cast.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [SpecialConversion]
        [Obsolete(SpecialOperationsUsageErrorMessage, true)]
        public static byte ToByte(double input)
        {
            return (byte)input;
        }

        /// <summary>
        /// Special cast.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [SpecialConversion]
        [Obsolete(SpecialOperationsUsageErrorMessage, true)]
        public static byte ToByte(float input)
        {
            return (byte)input;
        }

        /// <summary>
        /// Special cast.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [SpecialConversion]
        [Obsolete(SpecialOperationsUsageErrorMessage, true)]
        public static byte ToByte(int input)
        {
            return (byte)input;
        }

        /// <summary>
        /// Special cast.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [SpecialConversion]
        [Obsolete(SpecialOperationsUsageErrorMessage, true)]
        public static byte ToByte(char input)
        {
            return (byte)input;
        }
    }
}
