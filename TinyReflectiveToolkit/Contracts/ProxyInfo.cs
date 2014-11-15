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
    internal class ProxyInfo
    {
        public Type ProvidedType;
        public Type Contract;
        public List<MethodInfo> RequiredMethods;
        public List<MethodInfo> RequiredExplicitConversions;
        public List<MethodInfo> RequiredImplicitConversions;
        public List<MethodInfo> RequiredLeftSideAdditionOperators;
        public List<MethodInfo> RequiredRightSideAdditionOperators;
        public List<MethodInfo> RequiredLeftSideSubtractionOperators;
        public List<MethodInfo> RequiredRightSideSubtractionOperators;
        public List<MethodInfo> RequiredLeftSideMultiplicationOperators;
        public List<MethodInfo> RequiredRightSideMultiplicationOperators;
        public List<MethodInfo> RequiredLeftSideDivisionOperators;
        public List<MethodInfo> RequiredRightSideDivisionOperators;
        public List<MethodInfo> RequiredLeftSideModulusOperators;
        public List<MethodInfo> RequiredRightSideModulusOperators;
        public List<MethodInfo> RequiredLeftSideEqualityOperators;
        public List<MethodInfo> RequiredRightSideEqualityOperators;
        public List<MethodInfo> RequiredLeftSideInequalityOperators;
        public List<MethodInfo> RequiredRightSideInequalityOperators;
        public List<MethodInfo> RequiredLeftSideGreaterThanOperators;
        public List<MethodInfo> RequiredRightSideGreaterThanOperators;
        public List<MethodInfo> RequiredLeftSideLessThanOperators;
        public List<MethodInfo> RequiredRightSideLessThanOperators;

        public List<MethodInfo> FoundMethods;
        public List<Tuple<string, MethodInfo, int>> FoundExplicitConversions;
        public List<Tuple<string, MethodInfo, int>> FoundImplicitConversions;
        public List<Tuple<string, MethodInfo, int>> FoundLeftSideAdditionOperators;
        public List<Tuple<string, MethodInfo, int>> FoundRightSideAdditionOperators;
        public List<Tuple<string, MethodInfo, int>> FoundLeftSideSubtractionOperators;
        public List<Tuple<string, MethodInfo, int>> FoundRightSideSubtractionOperators;
        public List<Tuple<string, MethodInfo, int>> FoundLeftSideMultiplicationOperators;
        public List<Tuple<string, MethodInfo, int>> FoundRightSideMultiplicationOperators;
        public List<Tuple<string, MethodInfo, int>> FoundLeftSideDivisionOperators;
        public List<Tuple<string, MethodInfo, int>> FoundRightSideDivisionOperators;
        public List<Tuple<string, MethodInfo, int>> FoundLeftSideModulusOperators;
        public List<Tuple<string, MethodInfo, int>> FoundRightSideModulusOperators;
        public List<Tuple<string, MethodInfo, int>> FoundLeftSideEqualityOperators;
        public List<Tuple<string, MethodInfo, int>> FoundRightSideEqualityOperators;
        public List<Tuple<string, MethodInfo, int>> FoundLeftSideInequalityOperators;
        public List<Tuple<string, MethodInfo, int>> FoundRightSideInequalityOperators;
        public List<Tuple<string, MethodInfo, int>> FoundLeftSideGreaterThanOperators;
        public List<Tuple<string, MethodInfo, int>> FoundRightSideGreaterThanOperators;
        public List<Tuple<string, MethodInfo, int>> FoundLeftSideLessThanOperators;
        public List<Tuple<string, MethodInfo, int>> FoundRightSideLessThanOperators;

        public bool IsValid
        {
            get
            {
                if (RequiredMethods.Count != FoundMethods.Count) return false;
                if (RequiredExplicitConversions.Count != FoundExplicitConversions.Count) return false;
                if (RequiredImplicitConversions.Count != FoundImplicitConversions.Count) return false;

                var reqFields = typeof (ProxyInfo).GetFields().Where(x => x.Name.StartsWith("Required") && x.Name.EndsWith("Operators")).ToList();
                var foundFields = typeof (ProxyInfo).GetFields().Where(x => x.Name.StartsWith("Found") && x.Name.EndsWith("Operators")).ToList();
                for (var i = 0; i < reqFields.Count; i++)
                {
                    var list1 = (List<MethodInfo>) reqFields[i].GetValue(this);
                    var list2 = (List<Tuple<string, MethodInfo, int>>) foundFields[i].GetValue(this);
                    if (list1.Count != list2.Count) return false;
                }

                if (FoundMethods.Any(x => x == null)) return false;
                if (AllFoundOperators.Any(x => x.Item2 == null)) return false;

                return true;
            }
        }

        public ProxyInfo()
        {
            typeof(ProxyInfo).GetFields()
                .Where(x => x.FieldType == typeof(List<MethodInfo>))
                .ForEach(x => x.SetValue(this, new List<MethodInfo>()));
            typeof(ProxyInfo).GetFields()
                .Where(x => x.FieldType == typeof(List<Tuple<string, MethodInfo, int>>))
                .ForEach(x => x.SetValue(this, new List<Tuple<string, MethodInfo, int>>()));
        }
        
        public IEnumerable<Tuple<string, MethodInfo, int>> AllFoundOperators
        {
            get
            {
                return typeof (ProxyInfo).GetFields()
                    .Where(x => x.FieldType == typeof (List<Tuple<string, MethodInfo, int>>))
                    .Select(x => (List<Tuple<string, MethodInfo, int>>) x.GetValue(this))
                    .SelectMany(x => x);
            }
        }

        public void ResolveBinaryOperators<TAttribute>(MethodInfo[] methodInfoList)
            where TAttribute : ExposeBinaryOperatorAttribute
        {
            var attributeName = typeof (TAttribute).Name.Replace("Attribute", "");
            var leftSideField = typeof (ProxyInfo).GetFields().Single(x => x.Name == "RequiredLeftSide" + attributeName + "Operators");
            var rightSideField = typeof(ProxyInfo).GetFields().Single(x => x.Name == "RequiredRightSide" + attributeName + "Operators");
            leftSideField.SetValue(this,
                methodInfoList.WithAttribute<TAttribute>(x => x.OperatorSide == OpSide.ThisLeft).ToList());
            rightSideField.SetValue(this,
                methodInfoList.WithAttribute<TAttribute>(x => x.OperatorSide == OpSide.ThisRight).ToList());
        }

        private List<MethodInfo> GetReqList<TAttribute>(string side)
            where TAttribute : ExposeBinaryOperatorAttribute
        {
            var attributeName = typeof(TAttribute).Name.Replace("Attribute", "");
            var field =
                typeof(ProxyInfo).GetFields().Single(x => x.Name == "Required" + side + "Side" + attributeName + "Operators");
            return (List<MethodInfo>)field.GetValue(this);
        }
        public List<MethodInfo> GetReqLeft<TAttribute>()
            where TAttribute : ExposeBinaryOperatorAttribute
        {
            return GetReqList<TAttribute>("Left");
        }
        public List<MethodInfo> GetReqRight<TAttribute>()
            where TAttribute : ExposeBinaryOperatorAttribute
        {
            return GetReqList<TAttribute>("Right");
        }

        private List<Tuple<string, MethodInfo, int>> GetFoundList<TAttribute>(string side)
            where TAttribute : ExposeBinaryOperatorAttribute
        {
            var attributeName = typeof(TAttribute).Name.Replace("Attribute", "");
            var field =
                typeof(ProxyInfo).GetFields().Single(x => x.Name == "Found" + side + "Side" + attributeName + "Operators");
            return (List<Tuple<string, MethodInfo, int>>)field.GetValue(this);
        }
        public List<Tuple<string, MethodInfo, int>> GetFoundLeft<TAttribute>()
            where TAttribute : ExposeBinaryOperatorAttribute
        {
            return GetFoundList<TAttribute>("Left");
        }
        public List<Tuple<string, MethodInfo, int>> GetFoundRight<TAttribute>()
            where TAttribute : ExposeBinaryOperatorAttribute
        {
            return GetFoundList<TAttribute>("Right");
        }
    }
}
