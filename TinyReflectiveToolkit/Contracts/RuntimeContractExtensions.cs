/*
 * TinyReflectiveToolkit
 * Copyright (C) 2014  Theodoros Chatzigiannakis
 * 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System;

namespace TinyReflectiveToolkit.Contracts
{
    /// <summary>
    /// Extension methods enabling post-build interface implementation.
    /// </summary>
    public static class RuntimeContractExtensions
    {
        private static readonly ContractProvider DefaultContractProvider = new ContractProvider("Default");

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

        /// <summary>
        /// Checks whether the provided type satisfies a specified contract.
        /// </summary>
        /// <typeparam name="TContract"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool AsTypeSaftisfies<TContract>(this Type type)
            where TContract : class
        {
            return DefaultContractProvider.CheckIfSatisfies<TContract>(type).Item1;
        }
    }
}
