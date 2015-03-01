using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TinyReflectiveToolkit.Contracts;
using TinyReflectiveToolkit.Contracts.Premade;

namespace TinyReflectiveToolkit
{
    /// <summary>
    /// Offers methods to convert an object to another, including applying custom and special conversions.
    /// </summary>
    public static class DynamicConversion
    {
        /// <summary>
        /// Checks whether this object's runtime type has an explicit or implicit conversion to the provided type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool CanBeConvertedTo<T>(object obj)
        {
            return obj.Satisfies<ICastableTo<T>>() || obj.Satisfies<IImplicitlyConvertibleTo<T>>();
        }

        /// <summary>
        /// Performs the explicit or implicit conversion of the object's runtime type to the provided type, or throws if none is found.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T ConvertTo<T>(object obj)
        {
            if (obj.Satisfies<ICastableTo<T>>()) return obj.ToContract<ICastableTo<T>>().Cast();
            if (obj.Satisfies<IImplicitlyConvertibleTo<T>>())
                return obj.ToContract<IImplicitlyConvertibleTo<T>>().PerformImplicitConversion();
            throw new InvalidCastException();
        }
    }
}
