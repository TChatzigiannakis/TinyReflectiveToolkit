/*
 * TinyReflectiveToolkit
 * Copyright (C) 2014-2015  Theodoros Chatzigiannakis
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
        /// Returns a runtime-generated proxy that implements the specified interface and forwards method calls to the given object - even if the relationship between the object's type and the interface wasn't declared at build time.
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
        /// Checks whether the runtime type of the provided instance satisfies the specified contract, using a default contract provider.
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
        /// Checks whether the runtime type of the provided instance satisfies the specified contract, using a default contract provider.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="contract"></param>
        /// <returns></returns>
        public static bool Satisfies(this object obj, Type contract)
        {
            return DefaultContractProvider.CheckIfSatisfies(obj, contract);
        }

        /// <summary>
        /// Checks whether the provided type satisfies the specified contract, using a default contract provider.
        /// </summary>
        /// <typeparam name="TContract"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool AsTypeSaftisfies<TContract>(this Type type)
            where TContract : class
        {
            return DefaultContractProvider.CheckIfSatisfies<TContract>(type);
        }

        /// <summary>
        /// Checks whether the provided type satisfies the specified contract, using a default contract provider.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="contract"></param>
        /// <returns></returns>
        public static bool AsTypeSatisfies(this Type type, Type contract)
        {
            return DefaultContractProvider.CheckIfSatisfies(type, contract);
        }

        /// <summary>
        /// Returns a runtime-generated proxy that implements the specified interface and forwards method calls to the null instance - even if the relationship between the type and the interface wasn't declared at build time.
        /// </summary>
        /// <returns>The static contract.</returns>
        /// <param name="type">Type.</param>
        /// <typeparam name="TContract">The 1st type parameter.</typeparam>
        public static TContract ToStaticContract<TContract>(this Type type)
            where TContract : class
        {
            return DefaultContractProvider.GenerateNullContractInstance<TContract>(type);
        }
    }
}
