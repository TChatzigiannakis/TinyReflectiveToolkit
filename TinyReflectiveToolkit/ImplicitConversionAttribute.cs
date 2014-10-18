using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TinyReflectiveToolkit
{
    /// <summary>
    /// Declares that this method will return the result of the implicit conversion of the given type to the method's return type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class ImplicitConversionAttribute : Attribute
    {
    }
}
