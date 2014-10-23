using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TinyReflectiveToolkit.Contracts.Premade
{
    /// <summary>
    /// A contract for classes that can be cast to a provided type.
    /// </summary>
    public interface ICastableTo<T>
    {
        /// <summary>
        /// Returns the result of a type's explicit cast to the contract's type argument.
        /// </summary>
        /// <returns></returns>
        [Cast]
        T Cast();
    }
}
