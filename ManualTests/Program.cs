﻿/*
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
using TinyReflectiveToolkit;
using TinyReflectiveToolkitTests;

namespace ManualTests
{
    class Program
    {
        static void Main(string[] args)
        {
            var contracts = new Contracts();
            //contracts.SelfCastable();
            //contracts.GenericContract();
            //contracts.CastableToInt();
            //contracts.FailingContract();
            contracts.VoidContract();
            //contracts.ParameterizedContract();
            //contracts.Overloads();
            //contracts.ExplicitConversionOperator();
            //contracts.ImplicitConversionOperator();
            //contracts.GetProperties();
        }        
    }
}
