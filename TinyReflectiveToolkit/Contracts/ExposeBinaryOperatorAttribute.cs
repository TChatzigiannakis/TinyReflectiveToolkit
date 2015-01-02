/*
 * TinyReflectiveToolkit
 * Copyright (C) 2014-2015  Theodoros Chatzigiannakis
 * 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

namespace TinyReflectiveToolkit.Contracts
{
    /// <summary>
    /// Base type for binary-operator-exposing attributes.
    /// </summary>
    public abstract class ExposeBinaryOperatorAttribute : ExposeOperatorAttribute
    {
        /// <summary>
        /// Specifies which side of the operator the internal object will be on.
        /// </summary>
        public OpSide OperatorSide { get; private set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="side"></param>
        protected ExposeBinaryOperatorAttribute(OpSide side)
        {
            OperatorSide = side;
        }
    }
}
