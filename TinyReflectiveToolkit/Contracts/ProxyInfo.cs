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
using System.Reflection;
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
        public List<MethodInfo> RequiredLeftSideEqualityOperators { get; set; }
        public List<MethodInfo> RequiredRightSideEqualityOperators { get; set; }
        public List<MethodInfo> RequiredLeftSideInequalityOperators { get; set; }
        public List<MethodInfo> RequiredRightSideInequalityOperators { get; set; }
        public List<MethodInfo> RequiredLeftSideGreaterThanOperators { get; set; }
        public List<MethodInfo> RequiredRightSideGreaterThanOperators { get; set; }
        public List<MethodInfo> RequiredLeftSideLessThanOperators { get; set; }
        public List<MethodInfo> RequiredRightSideLessThanOperators { get; set; }

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
        public List<Tuple<string, MethodInfo, int>> FoundLeftSideEqualityOperators { get; set; }
        public List<Tuple<string, MethodInfo, int>> FoundRightSideEqualityOperators { get; set; }
        public List<Tuple<string, MethodInfo, int>> FoundLeftSideInequalityOperators { get; set; }
        public List<Tuple<string, MethodInfo, int>> FoundRightSideInequalityOperators { get; set; }
        public List<Tuple<string, MethodInfo, int>> FoundLeftSideGreaterThanOperators { get; set; }
        public List<Tuple<string, MethodInfo, int>> FoundRightSideGreaterThanOperators { get; set; }
        public List<Tuple<string, MethodInfo, int>> FoundLeftSideLessThanOperators { get; set; }
        public List<Tuple<string, MethodInfo, int>> FoundRightSideLessThanOperators { get; set; }

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
                if (RequiredLeftSideEqualityOperators.Count != FoundLeftSideEqualityOperators.Count) return false;
                if (RequiredRightSideEqualityOperators.Count != FoundRightSideEqualityOperators.Count) return false;
                if (RequiredLeftSideInequalityOperators.Count != FoundLeftSideInequalityOperators.Count) return false;
                if (RequiredRightSideInequalityOperators.Count != FoundRightSideInequalityOperators.Count) return false;
                if (RequiredLeftSideGreaterThanOperators.Count != FoundLeftSideGreaterThanOperators.Count) return false;
                if (RequiredRightSideGreaterThanOperators.Count != FoundRightSideGreaterThanOperators.Count) return false;
                if (RequiredLeftSideLessThanOperators.Count != FoundLeftSideLessThanOperators.Count) return false;
                if (RequiredRightSideLessThanOperators.Count != FoundRightSideLessThanOperators.Count) return false;

                if (FoundMethods.Any(x => x == null)) return false;
                if (AllFoundOperators.Any(x => x.Item2 == null)) return false;

                return true;
            }
        }
    }
}
