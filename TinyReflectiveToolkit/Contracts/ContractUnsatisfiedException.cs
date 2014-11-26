/*
 * TinyReflectiveToolkit
 * Copyright (C) 2014  Theodoros Chatzigiannakis
 * 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System;
using System.Runtime.Serialization;

namespace TinyReflectiveToolkit.Contracts
{
    /// <summary>
    /// Represents an attempt to apply a contract to a type that doesn't satsify it.
    /// </summary>
    [Serializable]
    public class ContractUnsatisfiedException : InvalidOperationException
    {

        /// <summary>
        /// Initializes a new instance of this exception class.
        /// </summary>
        public ContractUnsatisfiedException()
        {
        }

        /// <summary>
        /// Initializes a new instance of this exception class with a specified error message.
        /// </summary>
        /// <param name="message"></param>
        public ContractUnsatisfiedException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of this exception class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public ContractUnsatisfiedException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of this exception class with serialized data.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected ContractUnsatisfiedException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
