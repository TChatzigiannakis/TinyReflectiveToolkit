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
using EnumerableExtensions;

namespace TinyReflectiveToolkit.Contracts
{
    internal sealed class ProxyInfo
    {
        public ProxyInfo()
        {
            typeof (ProxyInfo).GetProperties()
                .Where(x => x.PropertyType == typeof (List<MethodInfo>))
                .ForEach(x => x.SetValue(this, new List<MethodInfo>(), null));
            typeof (ProxyInfo).GetProperties()
                .Where(x => x.PropertyType == typeof (List<Tuple<string, MethodInfo, int>>))
                .ForEach(x => x.SetValue(this, new List<Tuple<string, MethodInfo, int>>(), null));            
        }

        public Type ProvidedType { get; set; }
        public Type Contract { get; set; }
        public List<MethodInfo> RequiredMethods { get; set; }
        public List<MethodInfo> RequiredExplicitConversions { get; set; }
        public List<MethodInfo> RequiredImplicitConversions { get; set; }
        public List<MethodInfo> RequiredLeftSideAdditionOperators { get; set; }
        public List<MethodInfo> RequiredRightSideAdditionOperators { get; set; }
        public List<MethodInfo> RequiredLeftSideSubtractionOperators { get; set; }
        public List<MethodInfo> RequiredRightSideSubtractionOperators { get; set; }
        public List<MethodInfo> RequiredLeftSideMultiplyOperators { get; set; }
        public List<MethodInfo> RequiredRightSideMultiplyOperators { get; set; }
        public List<MethodInfo> RequiredLeftSideDivisionOperators { get; set; }
        public List<MethodInfo> RequiredRightSideDivisionOperators { get; set; }
        public List<MethodInfo> RequiredLeftSideModulusOperators { get; set; }
        public List<MethodInfo> RequiredRightSideModulusOperators { get; set; }

        public List<MethodInfo> FoundMethods { get; set; }
        public List<Tuple<string, MethodInfo, int>> FoundExplicitConversions { get; set; }
        public List<Tuple<string, MethodInfo, int>> FoundImplicitConversions { get; set; }
        public List<Tuple<string, MethodInfo, int>> FoundLeftSideAdditionOperators { get; set; }
        public List<Tuple<string, MethodInfo, int>> FoundRightSideAdditionOperators { get; set; }
        public List<Tuple<string, MethodInfo, int>> FoundLeftSideSubtractionOperators { get; set; }
        public List<Tuple<string, MethodInfo, int>> FoundRightSideSubtractionOperators { get; set; }
        public List<Tuple<string, MethodInfo, int>> FoundLeftSideMultiplyOperators { get; set; }
        public List<Tuple<string, MethodInfo, int>> FoundRightSideMultiplyOperators { get; set; }
        public List<Tuple<string, MethodInfo, int>> FoundLeftSideDivisionOperators { get; set; }
        public List<Tuple<string, MethodInfo, int>> FoundRightSideDivisionOperators { get; set; }
        public List<Tuple<string, MethodInfo, int>> FoundLeftSideModulusOperators { get; set; }
        public List<Tuple<string, MethodInfo, int>> FoundRightSideModulusOperators { get; set; }

        public IEnumerable<Tuple<string, MethodInfo, int>> AllFoundOperators
        {
            get
            {
                return typeof (ProxyInfo).GetProperties()
                    .Where(x => x.PropertyType == typeof (List<Tuple<string, MethodInfo, int>>))
                    .Select(x => (List<Tuple<string, MethodInfo, int>>) x.GetValue(this, null))
                    .SelectMany(x => x);
            }
        }

        public bool IsValid
        {
            get
            {
                if (RequiredMethods.Count != FoundMethods.Count) return false;
                if (RequiredExplicitConversions.Count != FoundExplicitConversions.Count) return false;
                if (RequiredImplicitConversions.Count != FoundImplicitConversions.Count) return false;
                if (RequiredLeftSideAdditionOperators.Count != FoundLeftSideAdditionOperators.Count) return false;
                if (RequiredRightSideAdditionOperators.Count != FoundRightSideAdditionOperators.Count) return false;
                if (RequiredLeftSideSubtractionOperators.Count != FoundLeftSideSubtractionOperators.Count) return false;
                if (RequiredRightSideSubtractionOperators.Count != FoundRightSideSubtractionOperators.Count) return false;
                if (RequiredLeftSideMultiplyOperators.Count != FoundLeftSideMultiplyOperators.Count) return false;
                if (RequiredRightSideMultiplyOperators.Count != FoundRightSideMultiplyOperators.Count) return false;
                if (RequiredLeftSideDivisionOperators.Count != FoundLeftSideDivisionOperators.Count) return false;
                if (RequiredRightSideDivisionOperators.Count != FoundRightSideDivisionOperators.Count) return false;
                if (RequiredLeftSideModulusOperators.Count != FoundLeftSideModulusOperators.Count) return false;
                if (RequiredRightSideModulusOperators.Count != FoundRightSideModulusOperators.Count) return false;

                if (FoundMethods.Any(x => x == null)) return false;
                if (AllFoundOperators.Any(x => x.Item2 == null)) return false;

                return true;
            }
        }
    }
}
