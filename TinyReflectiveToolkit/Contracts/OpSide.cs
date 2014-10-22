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
using System.Text;

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
