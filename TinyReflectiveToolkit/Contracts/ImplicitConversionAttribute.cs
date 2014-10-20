using System;

namespace TinyReflectiveToolkit.Contracts
{
    /// <summary>
    /// Declares that this method will return the result of the implicit conversion of the given type to the method's return type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class ImplicitConversionAttribute : Attribute
    {
    }
}
