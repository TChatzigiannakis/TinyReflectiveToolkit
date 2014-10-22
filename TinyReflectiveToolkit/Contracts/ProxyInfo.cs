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
using System.Reflection;
using System.Text;

namespace TinyReflectiveToolkit.Contracts
{
    internal class ProxyInfo
    {
        public Type ProvidedType { get; set; }
        public Type Contract { get; set; }
        public List<MethodInfo> RequiredMethods { get; set; }
        public List<MethodInfo> RequiredExplicitConversions { get; set; }
        public List<MethodInfo> RequiredImplicitConversions { get; set; }
        public List<MethodInfo> FoundMethods { get; set; }
        public List<Tuple<string, MethodInfo>> FoundExplicitConversions { get; set; }
        public List<Tuple<string, MethodInfo>> FoundImplicitConversions { get; set; }
        public bool IsValid
        {
            get
            {
                if (RequiredMethods.Count != FoundMethods.Count) return false;
                if (RequiredExplicitConversions.Count != FoundExplicitConversions.Count) return false;
                if (RequiredImplicitConversions.Count != FoundImplicitConversions.Count) return false;
                if (FoundMethods.Any(x => x == null)) return false;
                if (FoundExplicitConversions.Any(x => x.Item2 == null)) return false;
                if (FoundImplicitConversions.Any(x => x.Item2 == null)) return false;
                return true;
            }
        }
    }
}
