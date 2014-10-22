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
        public ProxyInfo()
        {
            RequiredMethods = new List<MethodInfo>();
            RequiredExplicitConversions = new List<MethodInfo>();
            RequiredImplicitConversions = new List<MethodInfo>();
            RequiredLeftSideAdditionOperators = new List<MethodInfo>();
            RequiredRightSideAdditionOperators = new List<MethodInfo>();
            RequiredLeftSideSubtractionOperators = new List<MethodInfo>();
            RequiredRightSideSubtractionOperators = new List<MethodInfo>();
            RequiredLeftSideMultiplicationOperators = new List<MethodInfo>();
            RequiredRightSideMultiplicationOperators = new List<MethodInfo>();
            RequiredLeftSideDivisionOperators = new List<MethodInfo>();
            RequiredRightSideDivisionOperators = new List<MethodInfo>();

            FoundMethods = new List<MethodInfo>();
            FoundExplicitConversions = new List<Tuple<string, MethodInfo, int>>();
            FoundImplicitConversions = new List<Tuple<string, MethodInfo, int>>();
            FoundLeftSideAdditionOperators = new List<Tuple<string, MethodInfo, int>>();
            FoundRightSideAdditionOperators = new List<Tuple<string, MethodInfo, int>>();
            FoundLeftSideSubtractionOperators = new List<Tuple<string, MethodInfo, int>>();
            FoundRightSideSubtractionOperators = new List<Tuple<string, MethodInfo, int>>();
            FoundLeftSideMultiplicationOperators = new List<Tuple<string, MethodInfo, int>>();
            FoundRightSideMultiplicationOperators = new List<Tuple<string, MethodInfo, int>>();
            FoundLeftSideDivisionOperators = new List<Tuple<string, MethodInfo, int>>();
            FoundRightSideDivisionOperators = new List<Tuple<string, MethodInfo, int>>();
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
        public List<MethodInfo> RequiredLeftSideMultiplicationOperators { get; set; }
        public List<MethodInfo> RequiredRightSideMultiplicationOperators { get; set; }
        public List<MethodInfo> RequiredLeftSideDivisionOperators { get; set; }
        public List<MethodInfo> RequiredRightSideDivisionOperators { get; set; }

        public List<MethodInfo> FoundMethods { get; set; }
        public List<Tuple<string, MethodInfo, int>> FoundExplicitConversions { get; set; }
        public List<Tuple<string, MethodInfo, int>> FoundImplicitConversions { get; set; }
        public List<Tuple<string, MethodInfo, int>> FoundLeftSideAdditionOperators { get; set; }
        public List<Tuple<string, MethodInfo, int>> FoundRightSideAdditionOperators { get; set; }
        public List<Tuple<string, MethodInfo, int>> FoundLeftSideSubtractionOperators { get; set; }
        public List<Tuple<string, MethodInfo, int>> FoundRightSideSubtractionOperators { get; set; }
        public List<Tuple<string, MethodInfo, int>> FoundLeftSideMultiplicationOperators { get; set; }
        public List<Tuple<string, MethodInfo, int>> FoundRightSideMultiplicationOperators { get; set; }
        public List<Tuple<string, MethodInfo, int>> FoundLeftSideDivisionOperators { get; set; }
        public List<Tuple<string, MethodInfo, int>> FoundRightSideDivisionOperators { get; set; }

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
                if (RequiredLeftSideMultiplicationOperators.Count != FoundLeftSideMultiplicationOperators.Count) return false;
                if (RequiredRightSideMultiplicationOperators.Count != FoundRightSideMultiplicationOperators.Count) return false;
                if (RequiredLeftSideDivisionOperators.Count != FoundLeftSideDivisionOperators.Count) return false;
                if (RequiredRightSideDivisionOperators.Count != FoundRightSideDivisionOperators.Count) return false;

                if (FoundMethods.Any(x => x == null)) return false;
                if (FoundExplicitConversions.Any(x => x.Item2 == null)) return false;
                if (FoundImplicitConversions.Any(x => x.Item2 == null)) return false;
                if (FoundLeftSideAdditionOperators.Any(x => x.Item2 == null)) return false;
                if (FoundRightSideAdditionOperators.Any(x => x.Item2 == null)) return false;
                if (FoundLeftSideSubtractionOperators.Any(x => x.Item2 == null)) return false;
                if (FoundRightSideSubtractionOperators.Any(x => x.Item2 == null)) return false;
                if (FoundLeftSideMultiplicationOperators.Any(x => x.Item2 == null)) return false;
                if (FoundRightSideMultiplicationOperators.Any(x => x.Item2 == null)) return false;
                if (FoundLeftSideDivisionOperators.Any(x => x.Item2 == null)) return false;
                if (FoundRightSideDivisionOperators.Any(x => x.Item2 == null)) return false;

                return true;
            }
        }
    }
}
