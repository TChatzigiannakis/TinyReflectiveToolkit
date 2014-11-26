using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TinyReflectiveToolkit.Contracts.Premade
{
    /// <summary>
    /// A contract for types that can be parsed from a string.
    /// </summary>
    public interface IParsable
    {
        /// <summary>
        /// Returns the result of the parsing method of the runtime type from a string.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        [Static]
        object Parse(string s);
    }
}
