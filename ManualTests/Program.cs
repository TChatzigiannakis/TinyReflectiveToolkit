/*
 * TinyReflectiveToolkit
 * Copyright (C) 2014  Theodoros Chatzigiannakis
 * 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TinyReflectiveToolkit;
using TinyReflectiveToolkitTests;
using System.Reflection;

namespace ManualTests
{
    class Program
    {
        static void Main(string[] args)
        {
            var contractTests = new Contracts();
            contractTests.ContractInheritance();
        }
    }

}
