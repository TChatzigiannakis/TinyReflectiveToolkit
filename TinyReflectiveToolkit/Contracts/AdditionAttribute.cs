﻿/*
 * TinyReflectiveToolkit
 * Copyright (C) 2014-2015  Theodoros Chatzigiannakis
 * 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System;

namespace TinyReflectiveToolkit.Contracts
{
    /// <summary>
    /// Declares that instead of being an actual method implemented by the type of the provided object,
    /// this will be a new, automatically implemented method that returns the result of the
    /// application of the addition operator on the provided object and another object.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class AdditionAttribute : ExposeBinaryOperatorAttribute
    {
        /// <summary>
        /// Declares a proxy method to the operator, with the current object being on the specified side.
        /// </summary>
        /// <param name="side">The side on which the current instance will be provided to the addition operator (with the argument going to the other side).</param>
        public AdditionAttribute(OpSide side)
            : base(side)
        {
        }

        /// <summary>
        /// Declares a proxy method to the operator, with the current object being on the left.
        /// </summary>
        public AdditionAttribute() 
            : this(OpSide.ThisLeft)
        {            
        }
    }
}
