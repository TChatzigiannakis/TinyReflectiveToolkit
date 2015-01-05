using System;

namespace TinyReflectiveToolkit.Contracts.Premade
{
    /// <summary>
    /// A contract for types that can have a parameterizable string representation, using a System.IFormatProvider
    /// </summary>
    public interface IFormatRepresentable
    {
        /// <summary>
        /// Returns a parameterized string representation of the current object.
        /// </summary>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        string ToString(IFormatProvider formatProvider);
    }
}

