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
    /// Declares that instead of being an actual method implemented by the type of the provided object,
    /// this will be a new, automatically implemented method that returns the result of the
    /// explicit conversion of the given type to this method's return type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class CastAttribute : ExposeOperatorAttribute
    {
    }
}
