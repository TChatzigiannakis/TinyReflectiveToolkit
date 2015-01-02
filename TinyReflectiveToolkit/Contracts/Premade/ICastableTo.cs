/*
 * TinyReflectiveToolkit
 * Copyright (C) 2014-2015  Theodoros Chatzigiannakis
 * 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

namespace TinyReflectiveToolkit.Contracts.Premade
{
    /// <summary>
    /// A contract for types that can be cast to a provided type.
    /// </summary>
    public interface ICastableTo<T>
    {
        /// <summary>
        /// Returns the result of a type's explicit cast to the contract's type argument.
        /// </summary>
        /// <returns></returns>
        [Cast]
        T Cast();
    }
}
