using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TinyReflectiveToolkit.Contracts.Premade
{
    /// <summary>
    /// A contract that matches types that have an addition operator.
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public interface IAddable<TInput, TResult>
    {
        /// <summary>
        /// Returns the result of the addition operator between this type and another type.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        [Addition(OpSide.ThisLeft)]
        TResult Add(TInput a);

        /// <summary>
        /// Returns the result of the addition operator another type and this type.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        [Addition(OpSide.ThisRight)]
        TResult AddFromLeft(TInput a);
    }
}
