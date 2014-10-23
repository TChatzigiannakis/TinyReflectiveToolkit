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

namespace TinyReflectiveToolkit.Contracts
{
    /// <summary>
    /// Declares that instead of being an actual method implemented by the type of the provided object,
    /// this will be a new, automatically implemented method that returns the result of the
    /// application of the addition operator on the provided object and another object.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class AdditionAttribute : ExposeBinaryOperatorAttribute
    {
        /// <summary>
        /// The default constructor.
        /// </summary>
        /// <param name="side">The side on which the current instance will be provided to the multiplication operator (with the argument going to the other side).</param>
        public AdditionAttribute(OpSide side)
            : base(side)
        {
        }
    }
}
