using System;

namespace TinyReflectiveToolkit.Contracts.Premade
{
    /// <summary>
    /// A contract for types that can be parsed from a string in a parameterizable way, using a System.IFormatProvider
    /// </summary>
    public interface IFormatParsable
    {
        /// <summary>
        /// Returns the result of the parameterizable parsing method of the current type or the current object's runtime type.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        [Static]
        object Parse(string input, IFormatProvider formatProvider);
    }
}

