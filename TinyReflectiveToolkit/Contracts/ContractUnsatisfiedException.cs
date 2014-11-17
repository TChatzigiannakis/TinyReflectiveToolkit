using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

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
