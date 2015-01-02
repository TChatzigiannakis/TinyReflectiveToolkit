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
    /// A contract for types that can be parsed from a string.
    /// </summary>
    public interface IParsable
    {
        /// <summary>
        /// Returns the result of the parsing method of the runtime type from a string.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        [Static]
        object Parse(string s);
    }
}
