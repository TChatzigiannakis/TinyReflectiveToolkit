using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TinyReflectiveToolkit.Contracts.Premade
{
    /// <summary>
    /// A contract for types that define an implicit conversion to a provided type.
    /// </summary>
    interface IImplicitlyConvertibleTo<T>
    {
        /// <summary>
        /// Returns the result of a type's implicit conversion to the contract's type argument, performed on the current object.
        /// </summary>
        /// <returns></returns>
        [Implicit]
        T PerformImplicitConversion();
    }
}
