/*
 *  Tiny Reflective Toolkit
    Copyright (C) 2014  Theodoros Chatzigiannakis

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>. 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using EnumerableExtensions;

namespace TinyReflectiveToolkit.Contracts
{
    /// <summary>
    /// Extension methods enabling post-build interface implementation.
    /// </summary>
    public static class RuntimeContractExtensions
    {
        private static readonly ContractProvider DefaultContractProvider = new ContractProvider();

        /// <summary>
        /// Returns a runtime-generated proxy that implements a specified interface and forwards method calls to the given object - even if the relationship between the object's type and the interface wasn't declared at build time.
        /// </summary>
        /// <typeparam name="TContract"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static TContract ToContract<TContract>(this object obj)
            where TContract : class
        {
            return DefaultContractProvider.ConvertToContractInstance<TContract>(obj);
        }

        /// <summary>
        /// Checks whether the runtime type of an object structurally satisfies a specified contract.
        /// </summary>
        /// <typeparam name="TContract"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool Satisfies<TContract>(this object obj)
            where TContract : class
        {
            return DefaultContractProvider.CheckIfSatisfies<TContract>(obj);
        }

    }
}
