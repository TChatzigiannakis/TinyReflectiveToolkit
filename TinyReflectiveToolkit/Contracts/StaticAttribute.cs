using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TinyReflectiveToolkit.Contracts
{
    /// <summary>
    /// Declares that the type of the provided object must have a static method that matches the declared signature.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class StaticAttribute : Attribute
    {
    }
}
