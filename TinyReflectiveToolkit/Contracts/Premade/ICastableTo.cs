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

namespace TinyReflectiveToolkit.Contracts.Premade
{
    /// <summary>
    /// A contract for classes that can be cast to a provided type.
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
