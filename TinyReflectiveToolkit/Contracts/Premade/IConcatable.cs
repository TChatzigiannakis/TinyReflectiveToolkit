/*
 * TinyReflectiveToolkit
 * Copyright (C) 2014  Theodoros Chatzigiannakis
 * 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

namespace TinyReflectiveToolkit.Contracts.Premade
{
    /// <summary>
    /// A contract for types that can be concatenated to produce a string.
    /// </summary>
    public interface IConcatable
    {
        /// <summary>
        /// Returns the result of the type's concatenation with a string.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        [Addition(OpSide.ThisLeft)]
        string Concat(string s);

        /// <summary>
        /// Returns the result of a string's concatenation with the type.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        [Addition(OpSide.ThisRight)]
        string Prepend(string s);
    }
}
