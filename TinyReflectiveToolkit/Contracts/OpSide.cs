/*
 * TinyReflectiveToolkit
 * Copyright (C) 2014  Theodoros Chatzigiannakis
 * 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

namespace TinyReflectiveToolkit.Contracts
{
    /// <summary>
    /// Specifies where the runtime type appears in a binary operator.
    /// </summary>
    public enum OpSide
    {
        /// <summary>
        /// The runtime type appears on the left side of the binary operator and the method's argument appears on the right side.
        /// </summary>
        ThisLeft,
        /// <summary>
        /// The runtime type appears on the right side of the binary operator and the method's argument appears on the left side.
        /// </summary>
        ThisRight
    }
}
